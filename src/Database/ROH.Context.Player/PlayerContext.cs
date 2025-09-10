using Microsoft.EntityFrameworkCore;

using ROH.Context.Player.Entities.Characters;
using ROH.Context.Player.Entities.Guilds;
using ROH.Context.Player.Entities.Kingdoms;
using ROH.Context.Player.Interface;
using ROH.Context.Player.TypeConfiguration.Characters;
using ROH.Context.Player.TypeConfiguration.Guilds;
using ROH.Context.Player.TypeConfiguration.Kingdoms;

namespace ROH.Context.Player;

public class PlayerContext : DbContext, IPlayerContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_PLAYER");
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
        _ = modelBuilder.ApplyConfiguration(new GuildTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new MembersPositionTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ChampionTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new KingdomTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new RelationTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PlayerPositionTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PositionTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new RotationTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    public DbSet<AttackStatus> AttackStatuses { get; set; }

    public DbSet<Champion> Champions { get; set; }

    public DbSet<CharacterInventory> CharacterInventory { get; set; }

    public DbSet<Character> Characters { get; set; }

    public DbSet<CharacterSkill> CharacterSkills { get; set; }

    public DbSet<DefenseStatus> DefenseStatuses { get; set; }

    public DbSet<EquippedItems> EquippedItems { get; set; }

    public DbSet<Guild> Guilds { get; set; }

    public DbSet<KingdomRelation> KingdomRelations { get; set; }

    public DbSet<Kingdom> Kingdoms { get; set; }

    public DbSet<MembersPosition> MembersPositions { get; set; }

    public DbSet<HandRing> RingsEquipped { get; set; }

    public DbSet<Skill> Skills { get; set; }

    public DbSet<Status> Statuses { get; set; }

    public DbSet<PlayerPosition> PlayersPosition { get; set; }

    public DbSet<Player.Entities.Characters.Position> Positions { get; set; }

    public DbSet<Rotation> Rotations { get; set; }
}
