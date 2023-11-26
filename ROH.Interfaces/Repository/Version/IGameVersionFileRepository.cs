using ROH.Domain.Version;

namespace ROH.Interfaces.Repository.Version
{
    public interface IGameVersionFileRepository
    {
        Task<GameVersionFile?> GetFile(long id);

        Task<List<GameVersionFile>> GetFiles(GameVersion version);

        Task<List<GameVersionFile>> GetFiles(Guid versionGuid);

        Task SaveFile(GameVersionFile file);
    }
}