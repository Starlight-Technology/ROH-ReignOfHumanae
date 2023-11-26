using Microsoft.EntityFrameworkCore;

using ROH.Domain.Version;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Version;

namespace ROH.Repository.Version
{
    public class GameVersionFileRepository : IGameVersionFileRepository
    {
        private readonly ISqlContext _context;

        public GameVersionFileRepository(ISqlContext context)
        {
            _context = context;
        }

        public async Task<GameVersionFile?> GetFile(long id)
        {
            return await _context.GameVersionFiles.FindAsync(id);
        }

        public async Task<List<GameVersionFile>> GetFiles(GameVersion version)
        {
            long versionId = _context.GameVersions.FirstAsync(v => v.Guid == version.Guid).Result.Id;

            return await _context.GameVersionFiles.Where(v => v.IdVersion == versionId).ToListAsync();
        }

        public async Task<List<GameVersionFile>> GetFiles(Guid versionGuid)
        {
            long versionId = _context.GameVersions.FirstAsync(v => v.Guid == versionGuid).Result.Id;

            return await _context.GameVersionFiles.Where(v => v.IdVersion == versionId).ToListAsync();
        }

        public async Task SaveFile(GameVersionFile file)
        {
            file.GameVersion = await _context.GameVersions.FirstAsync(v => v.Guid == file.GameVersion!.Guid);

            _ = await _context.GameVersionFiles.AddAsync(file);
            _ = await _context.SaveChangesAsync();
        }
    }
}