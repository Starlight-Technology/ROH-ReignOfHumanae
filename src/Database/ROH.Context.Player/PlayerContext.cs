﻿using Microsoft.EntityFrameworkCore;

using ROH.Context.Player.Entities.Characters;
using ROH.Context.Player.Entities.Guilds;
using ROH.Context.Player.Entities.Kingdoms;
using ROH.Context.Player.TypeConfiguration.Characters;
using ROH.Context.Player.TypeConfiguration.Guilds;
using ROH.Context.Player.TypeConfiguration.Kingdoms;

namespace ROH.Context.Player;

public class PlayerContext : DbContext
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


        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    public required DbSet<AttackStatus> AttackStatuses { get; set; }

    public required DbSet<Champion> Champions { get; set; }

    public required DbSet<CharacterInventory> CharacterInventory { get; set; }

    public required DbSet<Character> Characters { get; set; }

    public required DbSet<CharacterSkill> CharacterSkills { get; set; }

    public required DbSet<DefenseStatus> DefenseStatuses { get; set; }

    public required DbSet<EquippedItems> EquippedItems { get; set; }

    public required DbSet<Guild> Guilds { get; set; }

    public required DbSet<KingdomRelation> KingdomRelations { get; set; }

    public required DbSet<Kingdom> Kingdoms { get; set; }

    public required DbSet<MembersPosition> MembersPositions { get; set; }

    public required DbSet<HandRing> RingsEquipped { get; set; }

    public required DbSet<Skill> Skills { get; set; }

    public required DbSet<Status> Statuses { get; set; }
}
