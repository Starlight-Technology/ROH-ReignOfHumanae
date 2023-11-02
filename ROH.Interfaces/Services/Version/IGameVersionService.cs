using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Interfaces.Services.Version
{
    public interface IGameVersionService
    {
        Task<DefaultResponse?> GetVersionByGuid(Guid versionGuid);

        Task<DefaultResponse> GetAllReleasedVersions(int take = 10, int page = 1);

        Task<DefaultResponse> GetAllVersions(int take = 10, int page = 1);

        Task<DefaultResponse> GetCurrentVersion();

        Task<DefaultResponse> NewVersion(GameVersionModel version);

        Task<bool> VerifyIfVersionExist(GameVersionModel version);
    }
}