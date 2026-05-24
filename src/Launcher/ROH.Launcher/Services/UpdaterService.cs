using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ROH.Utils.ApiConfiguration;
using ROH.StandardModels.Version;
using ROH.StandardModels.Response;
using ROH.Utils.Helpers;

namespace ROH.Launcher.Services
{
    // Simple updater that talks to Gateway to get current version and downloads files from VersionFile via Gateway
    public class UpdaterService
    {
        readonly Gateway _gateway = new();
        readonly ROH.Utils.ApiConfiguration.ApiConfigReader _apiConfig = new();
        readonly SettingsService _settings;
        readonly LauncherState _launcherState;

        public UpdaterService(SettingsService settings, LauncherState launcherState)
        {
            _settings = settings;
            _launcherState = launcherState;
        }

        // Get the current released version via gateway
        public async Task<GameVersionModel?> GetCurrentVersionAsync(CancellationToken cancellationToken = default, string? token = null)
        {
            DefaultResponse? resp = await _gateway.GetAsync<object?>(Gateway.Services.GetCurrentVersion, null, token ?? string.Empty, cancellationToken)
                .ConfigureAwait(true);
            if (resp == null || !resp.HttpStatus.IsSuccessStatusCode())
                return null;

            try
            {
                return resp.MapObjectResponse<GameVersionModel>().ObjectResponse as GameVersionModel;
            }
            catch
            {
                return null;
            }
        }

        // Get file list for a version via gateway
        public async Task<List<GameVersionFileListModel>?> GetFilesForVersionAsync(Guid versionGuid, CancellationToken cancellationToken = default, string? token = null)
        {
            DefaultResponse? resp = await _gateway.GetAsync(Gateway.Services.GetAllVersionFiles, new { VersionGuid = versionGuid.ToString() }, token ?? string.Empty, cancellationToken).ConfigureAwait(true);
            if (resp == null || !resp.HttpStatus.IsSuccessStatusCode())
                return null;

            try
            {
                // resp.ObjectResponse might already be deserialized dynamic; try converting
                string json = JsonConvert.SerializeObject(resp.ObjectResponse);
                return JsonConvert.DeserializeObject<List<GameVersionFileListModel>>(json);
            }
            catch
            {
                return null;
            }
        }

        // Download each file (Gateway returns a DefaultResponse with GameFileModel inside)
        public async Task DownloadFilesAsync(IEnumerable<GameVersionFileListModel> files, string destinationFolder, IProgress<double> progress, CancellationToken cancellationToken = default, string? token = null)
        {
            // total size unknown; track count
            var fileList = new List<GameVersionFileListModel>(files);
            int total = fileList.Count;
            if (total == 0)
            {
                progress.Report(100);
                return;
            }

            int completed = 0;
            using HttpClient client = new HttpClient();

            // use launcher token if none provided
            string? useToken = token ?? _launcherState.Token;
            if (!string.IsNullOrWhiteSpace(useToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", useToken);

            foreach (var file in fileList)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // first, check checksum from API
                string checksum = string.Empty;
                try
                {
                    var csResp = await _gateway.GetAsync<object?>(Gateway.Services.DownloadFile, new { FileGuid = file.FileGuid.ToString() }, token ?? string.Empty, cancellationToken).ConfigureAwait(true);
                    // fallback to FileChecksum endpoint
                    var checksumResp = await _gateway.GetAsync<object?>(Gateway.Services.DownloadFile, new { FileGuid = file.FileGuid.ToString() }, token ?? string.Empty, cancellationToken).ConfigureAwait(true);
                }
                catch
                {
                }

                // Request raw file endpoint directly from VersionFiles API (supports range)
                Uri? vfBase = null;
                try
                {
                    var apiUrls = _apiConfig.GetApiUrl();
                    vfBase = apiUrls.GetValueOrDefault(ROH.Utils.ApiConfiguration.ApiConfigReader.ApiUrl.VersionFile);
                }
                catch
                {
                    // fallback to config file in wwwroot
                    try
                    {
                        string cfg = System.IO.File.ReadAllText("wwwroot\\launcher-config.txt");
                        foreach (var line in cfg.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (line.StartsWith("GatewayBaseUrl", StringComparison.OrdinalIgnoreCase))
                            {
                                var kv = line.Split('=');
                                if (kv.Length == 2)
                                {
                                    var gatewayBase = kv[1].Trim();
                                    vfBase = new Uri(gatewayBase);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if (vfBase is null)
                {
                    completed++;
                    progress.Report((double)completed / total * 100);
                    continue;
                }

                Uri rawUri = new Uri(vfBase, $"DownloadFileRaw?fileGuid={file.FileGuid}");

                string targetPath = Path.Combine(destinationFolder, file.Name);
                Directory.CreateDirectory(destinationFolder);

                // resume support: if a partial file exists, request range
                long existingBytes = 0;
                if (File.Exists(targetPath))
                {
                    var fi = new FileInfo(targetPath);
                    existingBytes = fi.Length;
                }

                using var request = new HttpRequestMessage(HttpMethod.Get, rawUri);
                if (existingBytes > 0)
                    request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(existingBytes, null);

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(true);
                if (!response.IsSuccessStatusCode)
                {
                    completed++;
                    progress.Report((double)completed / total * 100);
                    continue;
                }

                long totalFileBytes = existingBytes;
                if (response.Content.Headers.ContentLength.HasValue)
                    totalFileBytes = existingBytes + response.Content.Headers.ContentLength.Value;

                await using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(true))
                {
                    // append if resuming
                    await using var fs = new FileStream(targetPath, FileMode.Append, FileAccess.Write, FileShare.None);
                    byte[] buffer = new byte[81920];
                    int read;
                    long downloadedThisFile = existingBytes;
                    while ((read = await responseStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken).ConfigureAwait(true)) > 0)
                    {
                        await fs.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(true);
                        downloadedThisFile += read;
                        // estimate overall progress by files completed + this file progress
                        double fileProgress = (double)downloadedThisFile / Math.Max(1, totalFileBytes);
                        double overall = ((double)completed + fileProgress) / total * 100;
                        progress.Report(Math.Min(100, overall));
                    }
                }

                // Verify checksum against server-provided checksum
                try
                {
                    Uri checksumUri = new Uri(vfBase, $"FileChecksum?fileGuid={file.FileGuid}");
                    using var csReq = new HttpRequestMessage(HttpMethod.Get, checksumUri);
                    if (!string.IsNullOrWhiteSpace(useToken))
                        csReq.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", useToken);

                    using var csResp = await client.SendAsync(csReq, cancellationToken).ConfigureAwait(true);
                    if (csResp.IsSuccessStatusCode)
                    {
                        string json = await csResp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);
                        try
                        {
                            var defaultResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ROH.StandardModels.Response.DefaultResponse>(json);
                            string serverChecksum = defaultResponse?.ObjectResponse?.ToString() ?? string.Empty;
                            // compute local checksum
                            using var sha = System.Security.Cryptography.SHA256.Create();
                            await using var fs2 = System.IO.File.OpenRead(targetPath);
                            byte[] localHash = sha.ComputeHash(fs2);
                            string localChecksum = BitConverter.ToString(localHash).Replace("-", string.Empty).ToLowerInvariant();

                            if (!string.IsNullOrWhiteSpace(serverChecksum) && !string.Equals(serverChecksum, localChecksum, StringComparison.OrdinalIgnoreCase))
                            {
                                // checksum mismatch
                                throw new InvalidOperationException($"Checksum mismatch for file {file.Name}");
                            }
                        }
                        catch (Exception)
                        {
                            // ignore parse errors and treat as success
                        }
                    }
                }
                catch (Exception ex)
                {
                    // bubble up - let caller show alert
                    throw new Exception("Checksum verification failed: " + ex.Message, ex);
                }

                completed++;
                progress.Report((double)completed / total * 100);
            }
        }
    }
}
