using Microsoft.EntityFrameworkCore;

using ROH.Domain.Version;
using ROH.Interfaces;

namespace ROH.Repository.Version
{
    public class GameVersionRepository
    {
        private readonly ISqlContext _context;

        public GameVersionRepository(ISqlContext context)
        {
            _context = context;
        }

        public async Task<GameVersion?> GetCurrentGameVersion() => await _context.GameVersions.OrderByDescending(v => v.ReleaseDate).FirstOrDefaultAsync();

        public async Task<GameVersion> SetNewGameVersion(GameVersion version)
        {
            await _context.GameVersions.AddAsync(version);
            await _context.SaveChangesAsync();

            return version;
        }

        public async Task<GameVersion> UpdateGameVersion(GameVersion version)
        {
            _context.GameVersions.Update(version);
            await _context.SaveChangesAsync();

            return version;
        }
    }
}
