//-----------------------------------------------------------------------
// <copyright file="AttackStatus.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Player.Entities.Characters;

public record AttackStatus(
    long IdCharacter,
    long LongRangedWeaponLevel,
    long MagicWeaponLevel,
    long OneHandedWeaponLevel,
    long TwoHandedWeaponLevel)
{
    public virtual Character? Character { get; set; }
}