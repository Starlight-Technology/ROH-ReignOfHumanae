using Microsoft.EntityFrameworkCore;

using ROH.Interfaces;
using ROH.Interfaces.Repository.GameFile;

namespace ROH.Repository.GameFile;
public class GameFileRepository(ISqlContext context) : IGameFileRepository
{
    public async Task<Domain.GameFiles.GameFile?> GetFile(long id) => await context.GameFiles.FindAsync(id);

    public async Task<Domain.GameFiles.GameFile?> GetFile(Guid fileGuid) => await context.GameFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid);

    public async Task SaveFile(Domain.GameFiles.GameFile file)
    {
        _ = await context.GameFiles.AddAsync(file);
        _ = await context.SaveChangesAsync();
    }
}
