//-----------------------------------------------------------------------
// <copyright file="Skill.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Effect;

namespace ROH.Domain.Characters;

public record Skill(
    long Id,
    long? Damage,
    long? Defense,
    long ManaCost,
    string? Animation,
    string Name,
    EffectType Type);