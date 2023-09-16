using ROH.Domain.Version;

namespace ROH.Interfaces.Repository.Version
{
    public interface IGameVersionRepository
    {
        Task<GameVersion?> GetVersionById(long versionId);

        Task<IList<GameVersion>?> GetAllReleasedVersions();

        Task<IList<GameVersion>?> GetAllVersions();

        Task<GameVersion?> GetCurrentGameVersion();

        Task<GameVersion> SetNewGameVersion(GameVersion version);

        Task<GameVersion> UpdateGameVersion(GameVersion version);

        Task<bool> VerifyIfExist(GameVersion version);
    }
}