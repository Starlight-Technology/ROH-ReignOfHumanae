using Microsoft.EntityFrameworkCore;

using ROH.Context.Character.Entities;

namespace ROH.Context.Character.Interface;

public interface ICharacterContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<AttackStatus> AttackStatuses { get; }
    DbSet<CharacterInventory> CharacterInventory { get; }
    DbSet<Entities.Character> Characters { get; }
    DbSet<CharacterSkill> CharacterSkills { get; }
    DbSet<DefenseStatus> DefenseStatuses { get; }
    DbSet<EquippedItems> EquippedItems { get; }
    DbSet<HandRing> RingsEquipped { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Status> Statuses { get; }
}
