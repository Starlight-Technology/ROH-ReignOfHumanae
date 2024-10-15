//-----------------------------------------------------------------------
// <copyright file="KingdomRelation.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace ROH.Domain.Kingdoms;

public record KingdomRelation(long Id, long IdKingdom, long IdKingdom2, Situation Situation)
{
    public virtual Kingdom? Kingdom { get; set; }

    public virtual Kingdom? Kingdom2 { get; set; }
}