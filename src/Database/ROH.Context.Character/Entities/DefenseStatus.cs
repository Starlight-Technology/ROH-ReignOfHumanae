//-----------------------------------------------------------------------
// <copyright file="DefenseStatus.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Character.Entities;

public record DefenseStatus(
    long IdCharacter,
    long ArcaneDefenseLevel,
    long DarknessDefenseLevel,
    long EarthDefenseLevel,
    long FireDefenseLevel,
    long LightDefenseLevel,
    long LightningDefenseLevel,
    long MagicDefenseLevel,
    long PhysicDefenseLevel,
    long WaterDefenseLevel,
    long WindDefenseLevel)
{
    public virtual Character? Character { get; set; }
}