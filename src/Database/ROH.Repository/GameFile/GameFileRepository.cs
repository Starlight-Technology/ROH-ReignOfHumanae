using Microsoft.EntityFrameworkCore;

using ROH.Interfaces;
using ROH.Interfaces.Repository.GameFile;

namespace ROH.Repository.GameFile;

public class GameFileRepository(ISqlContext context) : IGameFileRepository
{
    public async Task<Domain.GameFiles.GameFile?> GetFileAsync(long id) => await context.GameFiles.FindAsync(id);

    public async Task<Domain.GameFiles.GameFile?> GetFileAsync(Guid fileGuid) => await context.GameFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid);

    public async Task SaveFileAsync(Domain.GameFiles.GameFile file)
    {
        _ = await context.GameFiles.AddAsync(file);
        _ = await context.SaveChangesAsync();
    }

    public async Task UpdateFileAsync(Domain.GameFiles.GameFile file)
    {  
        context.GameFiles.Update(file);
        await context.SaveChangesAsync();
    }

}
