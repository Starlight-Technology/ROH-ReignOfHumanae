using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ROH.Context.Version;

public class VersionContextFactory : IDesignTimeDbContextFactory<VersionContext>
{
    public VersionContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VersionContext>();
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_VERSION");

        optionsBuilder.UseNpgsql(connectionString);

        return new VersionContext(optionsBuilder.Options);
    }
}
