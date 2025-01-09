using Microsoft.EntityFrameworkCore;

using ROH.Context.Item.Entities;
using ROH.Context.Item.TypeConfiguration;

namespace ROH.Context.Item;

public class ItemContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_ITEM");
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new EnchantmentsTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ItemEnchantmentsTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    public DbSet<Enchantment> Enchantments { get; set; }

    public DbSet<ItemEnchantment> ItemEnchantments { get; set; }

    public DbSet<Item.Entities.Item> Items { get; set; }
}
