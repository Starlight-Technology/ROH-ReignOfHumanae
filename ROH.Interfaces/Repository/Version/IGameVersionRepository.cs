using ROH.Domain.Paginator;
using ROH.Domain.Version;

namespace ROH.Interfaces.Repository.Version
{
    public interface IGameVersionRepository
    {
        Task<GameVersion?> GetVersionById(long versionId);

        Task<IList<GameVersion>?> GetAllReleasedVersions();

        Task<Paginated> GetAllVersions(int take = 10, int skip = 0);

        Task<GameVersion?> GetCurrentGameVersion();

        Task<GameVersion> SetNewGameVersion(GameVersion version);

        Task<GameVersion> UpdateGameVersion(GameVersion version);

        Task<bool> VerifyIfExist(GameVersion version);
    }
}