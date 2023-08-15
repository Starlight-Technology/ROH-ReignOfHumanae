using ROH.Domain.Version;

namespace ROH.Interfaces.Repository.Version
{
    public interface IGameVersionFileRepository
    {
        Task<GameVersionFile?> GetFile(long id);

        Task<List<GameVersionFile>> GetFiles(GameVersion version);

        Task SaveFile(GameVersionFile file);
    }
}