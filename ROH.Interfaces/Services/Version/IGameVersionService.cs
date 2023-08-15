using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Interfaces.Services.Version
{
    public interface IGameVersionService
    {
        Task<DefaultResponse?> GetVersionById(long idVersion);

        Task<DefaultResponse> GetAllReleasedVersions();

        Task<DefaultResponse> GetAllVersions();

        Task<DefaultResponse> GetCurrentVersion();

        Task<DefaultResponse> NewVersion(GameVersionModel version);

        Task<bool> VerifyIfVersionExist(GameVersionModel version);
    }
}