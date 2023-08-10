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

        public async Task<GameVersionFile?> GetFile(long id) => await _context.GameVersionFiles.FindAsync(id);

        public async Task<List<GameVersionFile>> GetFiles(GameVersion version) => await _context.GameVersionFiles.Where(v => v.IdVersion == version.Id).ToListAsync();

        public async Task SaveFile(GameVersionFile file)
        {
            await _context.GameVersionFiles.AddAsync(file);
            await _context.SaveChangesAsync();
        }
    }
}
