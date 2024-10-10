//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace ROH.Domain.Characters;

public record Status(
    long IdCharacter,
    long Level,
    long MagicLevel,
    long FullCarryWeight,
    long CurrentCarryWeight,
    long FullHealth,
    long CurrentHealth,
    long FullMana,
    long CurrentMana,
    long FullStamina,
    long CurrentStamina)
{
    public virtual Character? Character { get; set; }
}