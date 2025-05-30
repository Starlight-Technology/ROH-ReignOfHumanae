//-----------------------------------------------------------------------
// <copyright file="Character.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Player.Entities.Guilds;
using ROH.Context.Player.Entities.Kingdoms;

namespace ROH.Context.Player.Entities.Characters;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="GuidAccount"></param> use GUID from User not account, temporary
/// <param name="IdGuild"></param>
/// <param name="IdKingdom"></param>
/// <param name="Guid"></param>
/// <param name="Name"></param>
/// <param name="Race"></param>
public record Character(long Id, Guid GuidAccount, long? IdGuild, long? IdKingdom, Guid Guid, string? Name, Race Race)
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public virtual AttackStatus? AttackStatus { get; set; }

    public virtual DefenseStatus? DefenseStatus { get; set; }

    public virtual EquippedItems? EquippedItems { get; set; }

    public virtual Guild? Guild { get; set; }

    public virtual ICollection<CharacterInventory>? Inventory { get; set; }

    public virtual Kingdom? Kingdom { get; set; }

    public virtual ICollection<CharacterSkill>? Skills { get; set; }

    public virtual Status? Status { get; set; }

    public virtual PlayerPosition? PlayerPosition { get; set; }
}