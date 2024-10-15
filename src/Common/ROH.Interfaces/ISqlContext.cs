//-----------------------------------------------------------------------
// <copyright file="ISqlContext.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Domain.Accounts;
using ROH.Domain.Characters;
using ROH.Domain.GameFiles;
using ROH.Domain.Guilds;
using ROH.Domain.Items;
using ROH.Domain.Kingdoms;
using ROH.Domain.Logging;
using ROH.Domain.Version;

namespace ROH.Interfaces;

public interface ISqlContext
{
    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Account> Accounts { get; }

    DbSet<AttackStatus> AttackStatuses { get; }

    DbSet<Champion> Champions { get; }

    DbSet<CharacterInventory> CharacterInventory { get; }

    DbSet<Character> Characters { get; }

    DbSet<CharacterSkill> CharacterSkills { get; }

    DbSet<DefenseStatus> DefenseStatuses { get; }

    DbSet<Enchantment> Enchantments { get; }

    DbSet<EquippedItems> EquippedItems { get; }

    DbSet<GameFile> GameFiles { get; }

    DbSet<GameVersionFile> GameVersionFiles { get; }

    DbSet<GameVersion> GameVersions { get; }

    DbSet<Guild> Guilds { get; }

    DbSet<ItemEnchantment> ItemEnchantments { get; }

    DbSet<Item> Items { get; }

    DbSet<KingdomRelation> KingdomRelations { get; }

    DbSet<Kingdom> Kingdoms { get; }

    DbSet<Log> Logs { get; }

    DbSet<MembersPosition> MembersPositions { get; }

    DbSet<HandRing> RingsEquipped { get; }

    DbSet<Skill> Skills { get; }

    DbSet<Status> Statuses { get; }

    DbSet<User> Users { get; }
}