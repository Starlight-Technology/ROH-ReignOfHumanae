using ROH.Domain.Version;
using ROH.Models.Response;

namespace ROH.Interfaces.Services
{
    public interface IGameVersionService
    {
        Task<DefaultResponse> GetAllReleasedVersions();
        Task<DefaultResponse> GetAllVersions();
        Task<DefaultResponse> NewVersion(GameVersion version);
        Task<bool> VerifyIfVersionExist(GameVersion version);
    }
}