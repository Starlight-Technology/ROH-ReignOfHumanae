using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;

using Newtonsoft.Json;

using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.ApiConfiguration;
using ROH.Utils.Helpers;

namespace ROH.Launcher.Services
{
    public class UpdaterService
    {
        readonly ApiConfigReader _apiConfig = new();
        readonly Gateway _gateway = new();

        public async Task DownloadFilesAsync(
            IEnumerable<GameVersionFileModel> files,
            string destinationFolder,
            IProgress<UpdateProgressInfo> progress,
            CancellationToken cancellationToken = default,
            string? token = null)
        {
            List<GameVersionFileModel> fileList = files
                .Where(file => file.Active || file.Guid != Guid.Empty)
                .ToList();

            if (fileList.Count == 0)
            {
                progress.Report(
                    new UpdateProgressInfo
                    {
                        Percentage = 100,
                        Message = "Nenhum arquivo novo encontrado",
                    });
                return;
            }

            if (string.IsNullOrWhiteSpace(destinationFolder))
                throw new InvalidOperationException("A pasta de instalacao nao foi configurada.");

            Directory.CreateDirectory(destinationFolder);
            Uri versionFileBase = GetVersionFileBaseUrl();

            using HttpClient client = CreateHttpClient(token);
            string destinationRoot = Path.GetFullPath(destinationFolder);

            for (int index = 0; index < fileList.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                GameVersionFileModel file = fileList[index];
                string relativePath = BuildDestinationRelativePath(file);
                string targetPath = Path.GetFullPath(Path.Combine(destinationRoot, relativePath));

                if (!IsPathInsideRoot(destinationRoot, targetPath))
                    throw new InvalidOperationException($"Caminho de arquivo invalido: {file.Name}");

                string? targetDirectory = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrWhiteSpace(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                progress.Report(
                    new UpdateProgressInfo
                    {
                        Percentage = ((double)index / fileList.Count) * 100,
                        CompletedFiles = index,
                        TotalFiles = fileList.Count,
                        CurrentFileName = file.Name,
                        Message = "Baixando arquivos do jogo",
                    });

                await DownloadSingleFileAsync(
                        client,
                        versionFileBase,
                        file.Guid,
                        file.Name,
                        targetPath,
                        index,
                        fileList.Count,
                        progress,
                        cancellationToken)
                    .ConfigureAwait(true);

                await VerifyChecksumAsync(client, versionFileBase, file.Guid, targetPath, cancellationToken).ConfigureAwait(true);

                progress.Report(
                    new UpdateProgressInfo
                    {
                        Percentage = ((double)(index + 1) / fileList.Count) * 100,
                        CompletedFiles = index + 1,
                        TotalFiles = fileList.Count,
                        CurrentFileName = file.Name,
                        Message = "Aplicando atualizacao",
                    });
            }
        }

        public async Task<GameVersionModel?> GetCurrentVersionAsync(
            CancellationToken cancellationToken = default,
            string? token = null)
        {
            DefaultResponse? resp = await _gateway.GetAsync<object?>(
                    Gateway.Services.GetCurrentVersion,
                    null,
                    token ?? string.Empty,
                    cancellationToken)
                .ConfigureAwait(true);

            if (resp == null || !resp.HttpStatus.IsSuccessStatusCode())
                return null;

            return ConvertObjectResponse<GameVersionModel>(resp);
        }

        public async Task<List<GameVersionFileModel>?> GetFilesForVersionAsync(
            Guid versionGuid,
            CancellationToken cancellationToken = default,
            string? token = null)
        {
            DefaultResponse? resp = await _gateway.GetAsync(
                    Gateway.Services.GetAllVersionFiles,
                    new { VersionGuid = versionGuid.ToString() },
                    token ?? string.Empty,
                    cancellationToken)
                .ConfigureAwait(true);

            if (resp == null || !resp.HttpStatus.IsSuccessStatusCode())
                return null;

            return ConvertObjectResponse<List<GameVersionFileModel>>(resp);
        }

        static T? ConvertObjectResponse<T>(DefaultResponse response)
        {
            return ConvertPayload<T>(response.ObjectResponse);
        }

        static T? ConvertPayload<T>(object? payload)
        {
            if (payload is null)
                return default;

            if (payload is T typed)
                return typed;

            string json = payload is string raw
                ? raw
                : JsonConvert.SerializeObject(payload);

            DefaultResponse? nestedResponse = TryDeserialize<DefaultResponse>(json);
            if (nestedResponse?.ObjectResponse is not null && LooksLikeDefaultResponse(json))
                return ConvertPayload<T>(nestedResponse.ObjectResponse);

            return TryDeserialize<T>(json);
        }

        static bool LooksLikeDefaultResponse(string json) =>
            json.Contains("ObjectResponse", StringComparison.OrdinalIgnoreCase) ||
            json.Contains("objectResponse", StringComparison.OrdinalIgnoreCase);

        static T? TryDeserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }
        }

        static HttpClient CreateHttpClient(string? token)
        {
            HttpClient client = new();
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        static async Task DownloadSingleFileAsync(
            HttpClient client,
            Uri versionFileBase,
            Guid fileGuid,
            string fileName,
            string targetPath,
            int fileIndex,
            int totalFiles,
            IProgress<UpdateProgressInfo> progress,
            CancellationToken cancellationToken)
        {
            Uri rawUri = new(versionFileBase, $"DownloadFileRaw?fileGuid={fileGuid}");
            long existingBytes = File.Exists(targetPath) ? new FileInfo(targetPath).Length : 0;

            using HttpRequestMessage request = new(HttpMethod.Get, rawUri);
            if (existingBytes > 0)
                request.Headers.Range = new RangeHeaderValue(existingBytes, null);

            using HttpResponseMessage response = await client
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(true);

            if (response.StatusCode == HttpStatusCode.OK && existingBytes > 0)
                existingBytes = 0;

            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.PartialContent)
            {
                throw new InvalidOperationException($"Falha ao baixar '{fileName}': {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            long contentLength = response.Content.Headers.ContentLength ?? 0;
            long totalFileBytes = Math.Max(1, existingBytes + contentLength);
            FileMode fileMode = existingBytes > 0 && response.StatusCode == HttpStatusCode.PartialContent
                ? FileMode.Append
                : FileMode.Create;

            await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(true);
            await using FileStream outputStream = new(targetPath, fileMode, FileAccess.Write, FileShare.None);

            byte[] buffer = new byte[81920];
            int read;
            long downloadedBytes = existingBytes;

            while ((read = await responseStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)
                       .ConfigureAwait(true)) > 0)
            {
                await outputStream.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(true);
                downloadedBytes += read;

                double fileProgress = (double)downloadedBytes / totalFileBytes;
                double overall = ((double)fileIndex + Math.Clamp(fileProgress, 0, 1)) / totalFiles * 100;

                progress.Report(
                    new UpdateProgressInfo
                    {
                        Percentage = overall,
                        CompletedFiles = fileIndex,
                        TotalFiles = totalFiles,
                        CurrentFileName = fileName,
                        Message = "Baixando arquivos do jogo",
                    });
            }
        }

        Uri GetVersionFileBaseUrl()
        {
            try
            {
                Dictionary<ApiConfigReader.ApiUrl, Uri> apiUrls = _apiConfig.GetApiUrl();
                Uri? uri = apiUrls.GetValueOrDefault(ApiConfigReader.ApiUrl.VersionFile);
                if (uri is not null)
                    return uri;
            }
            catch
            {
            }

            throw new InvalidOperationException("Nao foi possivel localizar a URL do servico de arquivos.");
        }

        static string NormalizeRelativePath(string fileName)
        {
            string safeName = string.IsNullOrWhiteSpace(fileName) ? "download.bin" : fileName;
            return safeName
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar)
                .TrimStart(Path.DirectorySeparatorChar);
        }

        static string BuildDestinationRelativePath(GameVersionFileModel file)
        {
            string relativeName = NormalizeRelativePath(file.Name);

            if (string.IsNullOrWhiteSpace(file.Path))
                return relativeName;

            string targetFolder = NormalizeRelativePath(file.Path);
            string fileName = Path.GetFileName(relativeName);

            return NormalizeRelativePath(Path.Combine(targetFolder, fileName));
        }

        static bool IsPathInsideRoot(string rootPath, string filePath)
        {
            string rootWithSeparator = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            return filePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase);
        }

        static async Task VerifyChecksumAsync(
            HttpClient client,
            Uri versionFileBase,
            Guid fileGuid,
            string targetPath,
            CancellationToken cancellationToken)
        {
            Uri checksumUri = new(versionFileBase, $"FileChecksum?fileGuid={fileGuid}");
            using HttpResponseMessage checksumResponse = await client.SendAsync(
                    new HttpRequestMessage(HttpMethod.Get, checksumUri),
                    cancellationToken)
                .ConfigureAwait(true);

            if (!checksumResponse.IsSuccessStatusCode)
                return;

            string json = await checksumResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);
            DefaultResponse? defaultResponse = JsonConvert.DeserializeObject<DefaultResponse>(json);
            string serverChecksum = defaultResponse?.ObjectResponse?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(serverChecksum))
                return;

            await using FileStream fs = File.OpenRead(targetPath);
            byte[] localHash = await SHA256.HashDataAsync(fs, cancellationToken).ConfigureAwait(true);
            string localChecksum = BitConverter.ToString(localHash).Replace("-", string.Empty).ToLowerInvariant();

            if (!string.Equals(serverChecksum, localChecksum, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Falha na verificacao do arquivo '{Path.GetFileName(targetPath)}'.");
        }
    }
}
