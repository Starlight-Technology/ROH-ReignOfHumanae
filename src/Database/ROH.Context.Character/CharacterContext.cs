using Microsoft.EntityFrameworkCore;

using ROH.Context.Character.Entities;
using ROH.Context.Character.Interface;
using ROH.Context.Character.TypeConfiguration;

namespace ROH.Context.Character;

public class CharacterContext : DbContext, ICharacterContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_CHARACTER");
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new AttackStatusTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CharacterSkillTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CharacterTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new DefenseStatusTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new EquippedItemsTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new HandRingTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new InventoryTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SkillTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StatusTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    public DbSet<AttackStatus> AttackStatuses { get; set; }

    public DbSet<CharacterInventory> CharacterInventory { get; set; }

    public DbSet<Entities.Character> Characters { get; set; }

    public DbSet<CharacterSkill> CharacterSkills { get; set; }

    public DbSet<DefenseStatus> DefenseStatuses { get; set; }

    public DbSet<EquippedItems> EquippedItems { get; set; }

    public DbSet<HandRing> RingsEquipped { get; set; }

    public DbSet<Skill> Skills { get; set; }

    public DbSet<Status> Statuses { get; set; }
}
