using Microsoft.EntityFrameworkCore;

using ROH.Context.File.Entities;

namespace ROH.Context.File.Interface;

public interface IFileContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<GameFile> GameFiles { get; }
    DbSet<GameVersionFile> GameVersionFiles { get; }
}