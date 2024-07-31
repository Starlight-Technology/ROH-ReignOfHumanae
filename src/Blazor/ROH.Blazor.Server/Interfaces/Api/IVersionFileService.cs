using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Interfaces.Api;

public interface IVersionFileService
{
    Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel Model);

    Task<DefaultResponse?> GetAllVersionFiles(string VersionGuid);

    Task<DefaultResponse?> DownloadVersionFile(string FileGuid);
}