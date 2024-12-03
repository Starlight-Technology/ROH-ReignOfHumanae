using Microsoft.EntityFrameworkCore;

using ROH.Context.Version.Entities;
using ROH.Context.Version.Interface;
using ROH.Context.Version.TypeConfiguration;

namespace ROH.Context.Version;

public class VersionContext : DbContext, IVersionContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_VERSION");
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        _ = modelBuilder.ApplyConfiguration(new GameVersionTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);


    public DbSet<GameVersion> GameVersions { get; set; }

}
