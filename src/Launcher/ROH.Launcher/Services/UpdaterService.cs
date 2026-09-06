using Newtonsoft.Json;

using ROH.StandardModels.File;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System.Net;
using System.Net.Http.Headers;

namespace ROH.Launcher.Services;

public class UpdaterService
{
    private readonly SettingsService _settings;

    public UpdaterService(SettingsService settings) => _settings = settings;

    public async Task DownloadFilesAsync(
        IEnumerable<GameVersionFileModel> files,
        string destinationFolder,
        IProgress<UpdateProgressInfo> progress,
        CancellationToken cancellationToken = default,
        string? token = null)
    {
        List<GameVersionFileModel> fileList = [.. files.Where(file => file.Active && file.Guid != Guid.Empty)];

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
        Uri gatewayBase = GetGatewayBaseUrl();

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
                    Percentage = (double)index / fileList.Count * 100,
                    CompletedFiles = index,
                    TotalFiles = fileList.Count,
                    CurrentFileName = file.Name,
                    Message = "Baixando arquivos do jogo",
                });

            await DownloadSingleFileAsync(
                    client,
                    gatewayBase,
                    file.Guid,
                    file.Name,
                    targetPath,
                    index,
                    fileList.Count,
                    progress,
                    cancellationToken)
                .ConfigureAwait(true);

            progress.Report(
                new UpdateProgressInfo
                {
                    Percentage = (double)(index + 1) / fileList.Count * 100,
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
        Uri gatewayBase = GetGatewayBaseUrl();

        using HttpClient client = CreateHttpClient(token);

        HttpResponseMessage response = await client.GetAsync(
            new Uri(gatewayBase, "api/Version/GetCurrentVersion"),
            cancellationToken)
            .ConfigureAwait(true);

        if (!response.IsSuccessStatusCode)
            return null;

        string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);

        DefaultResponse? resp = JsonConvert.DeserializeObject<DefaultResponse>(json);

        return resp == null || !IsSuccessStatus(resp.HttpStatus) ? null : ConvertObjectResponse<GameVersionModel>(resp);
    }

    public async Task<List<GameVersionFileModel>?> GetFilesForVersionAsync(
        Guid versionGuid,
        CancellationToken cancellationToken = default,
        string? token = null)
    {
        Uri gatewayBase = GetGatewayBaseUrl();

        using HttpClient client = CreateHttpClient(token);

        string query = $"?VersionGuid={Uri.EscapeDataString(versionGuid.ToString())}";

        HttpResponseMessage response = await client.GetAsync(
            new Uri(gatewayBase, $"api/VersionFile/GetAllVersionFiles{query}"),
            cancellationToken)
            .ConfigureAwait(true);

        if (!response.IsSuccessStatusCode)
            return null;

        string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);

        DefaultResponse? resp = JsonConvert.DeserializeObject<DefaultResponse>(json);

        return resp == null || !IsSuccessStatus(resp.HttpStatus) ? null : ConvertObjectResponse<List<GameVersionFileModel>>(resp);
    }

    private static T? ConvertObjectResponse<T>(DefaultResponse response) => ConvertPayload<T>(response.ObjectResponse);

    private static T? ConvertPayload<T>(object? payload)
    {
        if (payload is null)
            return default;

        if (payload is T typed)
            return typed;

        string json = payload is string raw
            ? raw
            : JsonConvert.SerializeObject(payload);

        DefaultResponse? nestedResponse = TryDeserialize<DefaultResponse>(json);
        return nestedResponse?.ObjectResponse is not null && LooksLikeDefaultResponse(json)
            ? ConvertPayload<T>(nestedResponse.ObjectResponse)
            : TryDeserialize<T>(json);
    }

    private static bool LooksLikeDefaultResponse(string json) =>
        json.Contains("ObjectResponse", StringComparison.OrdinalIgnoreCase) ||
        json.Contains("objectResponse", StringComparison.OrdinalIgnoreCase);

    private static T? TryDeserialize<T>(string json)
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

    private static HttpClient CreateHttpClient(string? token)
    {
        HttpClientHandler handler = new HttpClientHandler();
#if DEBUG
        handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
#endif
        HttpClient client = new HttpClient(handler);
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    private static async Task DownloadSingleFileAsync(
        HttpClient client,
        Uri gatewayBase,
        Guid fileGuid,
        string fileName,
        string targetPath,
        int fileIndex,
        int totalFiles,
        IProgress<UpdateProgressInfo> progress,
        CancellationToken cancellationToken)
    {
        Uri downloadUri = new(gatewayBase, $"api/VersionFile/DownloadFile?fileGuid={fileGuid}");

        using HttpResponseMessage response = await client
            .GetAsync(downloadUri, cancellationToken)
            .ConfigureAwait(true);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Falha ao baixar '{fileName}': {(int)response.StatusCode} {response.ReasonPhrase}");

        string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);

        DefaultResponse? defaultResponse = JsonConvert.DeserializeObject<DefaultResponse>(json);

        if (defaultResponse == null || !IsSuccessStatus(defaultResponse.HttpStatus))
            throw new InvalidOperationException($"Falha ao baixar '{fileName}': resposta invalida do servidor.");

        GameFileModel? fileModel = defaultResponse.ObjectResponse != null
            ? JsonConvert.DeserializeObject<GameFileModel>(defaultResponse.ObjectResponse.ToString())
            : null;

        if (fileModel?.Content == null)
            throw new InvalidOperationException($"Falha ao baixar '{fileName}': conteudo vazio.");

        await File.WriteAllBytesAsync(targetPath, fileModel.Content, cancellationToken).ConfigureAwait(true);

        double overall = (double)(fileIndex + 1) / totalFiles * 100;

        progress.Report(
            new UpdateProgressInfo
            {
                Percentage = overall,
                CompletedFiles = fileIndex + 1,
                TotalFiles = totalFiles,
                CurrentFileName = fileName,
                Message = "Baixando arquivos do jogo",
            });
    }

    private Uri GetGatewayBaseUrl()
    {
        if (Uri.TryCreate(_settings.GatewayBaseUrl, UriKind.Absolute, out Uri? uri))
            return uri;

        throw new InvalidOperationException("Nao foi possivel localizar a URL do gateway.");
    }

    private static bool IsSuccessStatus(HttpStatusCode statusCode) => (int)statusCode >= 200 && (int)statusCode < 300;

    private static string NormalizeRelativePath(string fileName)
    {
        string safeName = string.IsNullOrWhiteSpace(fileName) ? "download.bin" : fileName;
        return safeName
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);
    }

    private static string BuildDestinationRelativePath(GameVersionFileModel file)
    {
        string relativeName = NormalizeRelativePath(file.Name);

        if (string.IsNullOrWhiteSpace(file.Path))
            return relativeName;

        string targetFolder = NormalizeRelativePath(file.Path);
        string fileName = Path.GetFileName(relativeName);

        return NormalizeRelativePath(Path.Combine(targetFolder, fileName));
    }

    private static bool IsPathInsideRoot(string rootPath, string filePath)
    {
        string rootWithSeparator = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        return filePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase);
    }
}
