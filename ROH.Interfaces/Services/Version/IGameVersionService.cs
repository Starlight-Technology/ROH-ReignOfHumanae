using ROH.Domain.Version;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Version
{
    public interface IGameVersionService
    {
        Task<DefaultResponse> GetAllReleasedVersions();
        Task<DefaultResponse> GetAllVersions();
        Task<DefaultResponse> GetCurrentVersion();
        Task<DefaultResponse> NewVersion(GameVersion version);
        Task<bool> VerifyIfVersionExist(GameVersion version);
    }
}