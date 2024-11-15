using Microsoft.EntityFrameworkCore;

using ROH.Context.Account.Entity;
using ROH.Context.Account.Interface;
using ROH.Context.Account.TypeConfiguration;

namespace ROH.Context.Account;

public class AccountContext : DbContext, IAccountContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_ACCOUNT");
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new UserTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    public DbSet<Entity.Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
}
