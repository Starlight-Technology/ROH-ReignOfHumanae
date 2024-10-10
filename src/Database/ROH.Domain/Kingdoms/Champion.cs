//-----------------------------------------------------------------------
// <copyright file="Champion.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Characters;

namespace ROH.Domain.Kingdoms;

public record Champion(long Id, long IdCharacter, long IdKingdom)
{
    public virtual Character? Character { get; set; }

    public virtual Kingdom? Kingdom { get; set; }
}