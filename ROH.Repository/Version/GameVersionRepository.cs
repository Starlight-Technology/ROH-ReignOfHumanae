using Microsoft.EntityFrameworkCore;

using ROH.Domain.Version;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Version;

namespace ROH.Repository.Version
{
    public class GameVersionRepository : IGameVersionRepository
    {
        private readonly ISqlContext _context;

        public GameVersionRepository(ISqlContext context)
        {
            _context = context;
        }

        public async Task<GameVersion?> GetVersionById(long versionId)
        {
            return await _context.GameVersions.FindAsync(versionId);
        }

        public async Task<GameVersion?> GetCurrentGameVersion()
        {
            return await _context.GameVersions.OrderByDescending(v => v.ReleaseDate).FirstOrDefaultAsync();
        }

        public async Task<IList<GameVersion>?> GetAllVersions()
        {
            return await _context.GameVersions.Where(v => v.Id > 0).ToListAsync();
        }

        public async Task<IList<GameVersion>?> GetAllReleasedVersions()
        {
            return await _context.GameVersions.Where(v => v.Released).ToListAsync();
        }

        public async Task<GameVersion> SetNewGameVersion(GameVersion version)
        {
            version.VersionDate = DateTime.UtcNow;
            _ = await _context.GameVersions.AddAsync(version);
            _ = await _context.SaveChangesAsync();

            return version;
        }

        public async Task<GameVersion> UpdateGameVersion(GameVersion version)
        {
            _ = _context.GameVersions.Update(version);
            _ = await _context.SaveChangesAsync();

            return version;
        }

        public async Task<bool> VerifyIfExist(GameVersion version) => await _context.GameVersions.AnyAsync(v => v.Release == version.Release && v.Review == version.Review && v.Version == version.Version);
    }
}