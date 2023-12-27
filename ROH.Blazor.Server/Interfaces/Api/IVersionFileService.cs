using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Interfaces.Api
{
    public interface IVersionFileService
    {
        Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel model);
        Task<DefaultResponse?> GetAllVersionFiles(string versionGuid);
    }
}