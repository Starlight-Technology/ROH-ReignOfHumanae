﻿using Microsoft.EntityFrameworkCore;

using ROH.Domain.Accounts;
using ROH.Domain.Characters;
using ROH.Domain.Guilds;
using ROH.Domain.Itens;
using ROH.Domain.Kingdoms;

namespace ROH.Interfaces
{
    public interface ISqlContext
    {
        DbSet<User> Users { get; }
        DbSet<Account> Accounts { get; }
        DbSet<AttackStatus> AttackStatuses { get; }
        DbSet<CharacterSkill> CharacterSkills { get; }
        DbSet<Character> Characters { get; }
        DbSet<DefenseStatus> DefenseStatuses { get; }
        DbSet<EquippedItens> EquipedItens { get; }
        DbSet<HandRing> RingsEquipped { get; }
        DbSet<CharacterInventory> CharacterInventory { get; }
        DbSet<Skill> Skills { get; }
        DbSet<Status> Statuses { get; }
        DbSet<Guild> Guilds { get; }
        DbSet<MembersPosition> MembersPositions { get; }
        DbSet<Enchantment> Enchantments { get; }
        DbSet<ItemEnchantment> ItemEnchantments { get; }
        DbSet<Item> Items { get; }
        DbSet<Champion> Champions { get; }
        DbSet<Kingdom> Kingdoms { get; }
        DbSet<KingdomRelation> KingdomRelations { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}