using Microsoft.EntityFrameworkCore;

namespace ROH.Context.Log.Interface;

public interface ILogContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Entities.Log> Logs { get; }
}
