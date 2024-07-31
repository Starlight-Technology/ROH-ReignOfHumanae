using Microsoft.EntityFrameworkCore;

using ROH.Domain.Version;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Version;

namespace ROH.Repository.Version;

public class GameVersionFileRepository(ISqlContext context) : IGameVersionFileRepository
{
    public async Task<GameVersionFile?> GetFile(long id) => await context.GameVersionFiles.FindAsync(id);

    public async Task<GameVersionFile?> GetFile(Guid fileGuid) => await context.GameVersionFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid);

    public async Task<List<GameVersionFile>> GetFiles(GameVersion version)
    {
        long versionId = context.GameVersions.FirstAsync(v => v.Guid == version.Guid).Result.Id;

        return await context.GameVersionFiles.Where(v => v.IdVersion == versionId).ToListAsync();
    }

    public async Task<List<GameVersionFile>> GetFiles(Guid versionGuid)
    {
        long versionId = context.GameVersions.FirstAsync(v => v.Guid == versionGuid).Result.Id;

        return await context.GameVersionFiles.Where(v => v.IdVersion == versionId).ToListAsync();
    }

    public async Task SaveFile(GameVersionFile file)
    {
        file.GameVersion = await context.GameVersions.FirstAsync(v => v.Guid == file.GameVersion!.Guid);

        _ = await context.GameVersionFiles.AddAsync(file);
        _ = await context.SaveChangesAsync();
    }
}