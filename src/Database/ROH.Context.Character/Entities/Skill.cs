//-----------------------------------------------------------------------
// <copyright file="Skill.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// <copyright file="Skill.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Character.Enums;

namespace ROH.Context.Character.Entities;

public record Skill(
    long Id,
    long? Damage,
    long? Defense,
    long ManaCost,
    string? Animation,
    string Name,
    EffectType Type);