using Microsoft.EntityFrameworkCore;

using ROH.Context.Player.Entities.Characters;
using ROH.Context.Player.Entities.Guilds;
using ROH.Context.Player.Entities.Kingdoms;

namespace ROH.Context.Player.Interface;

public interface IPlayerContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<AttackStatus> AttackStatuses { get; set; }
    DbSet<Champion> Champions { get; set; }
    DbSet<CharacterInventory> CharacterInventory { get; set; }
    DbSet<Character> Characters { get; set; }
    DbSet<CharacterSkill> CharacterSkills { get; set; }
    DbSet<DefenseStatus> DefenseStatuses { get; set; }
    DbSet<EquippedItems> EquippedItems { get; set; }
    DbSet<Guild> Guilds { get; set; }
    DbSet<KingdomRelation> KingdomRelations { get; set; }
    DbSet<Kingdom> Kingdoms { get; set; }
    DbSet<MembersPosition> MembersPositions { get; set; }
    DbSet<HandRing> RingsEquipped { get; set; }
    DbSet<Skill> Skills { get; set; }
    DbSet<Status> Statuses { get; set; }
    DbSet<PlayerPosition> PlayersPosition { get; set; }
    DbSet<Player.Entities.Characters.Position> Positions { get; set; }
    DbSet<Rotation> Rotations { get; set; }
}