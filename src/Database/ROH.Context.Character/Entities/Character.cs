//-----------------------------------------------------------------------
// <copyright file="Character.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace ROH.Context.Character.Entities;

public record Character(long Id, long IdAccount, long? IdGuild, long IdKingdom, Guid Guid, string? Name, Race Race)
{
    private DateTime dateCreated;

    public Character(
        long id,
        long idAccount,
        long? idGuild,
        long idKingdom,
        Guid guid,
        string? name,
        Race race,
        DateTime dateCreated) : this(id, idAccount, idGuild, idKingdom, guid, name, race) => this.dateCreated =
        dateCreated;

    public virtual AttackStatus? AttackStatus { get; set; }

    [SuppressMessage(
        "Blocker Code Smell",
        "S3237:\"value\" parameters should be used",
        Justification = "<Is defined on set.>")]
    public DateTime DateCreated { get => dateCreated; private set => dateCreated = DateTime.Now; }

    public virtual DefenseStatus? DefenseStatus { get; set; }

    public virtual EquippedItems? EquippedItems { get; set; }


    public virtual ICollection<CharacterInventory>? Inventory { get; set; }

    public virtual ICollection<CharacterSkill>? Skills { get; set; }

    public virtual Status? Status { get; set; }
}