//-----------------------------------------------------------------------
// <copyright file="KingdomRelation.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Player.Entities.Kingdoms;

public record KingdomRelation(long Id, long IdKingdom, long IdKingdom2, Situation Situation)
{
    public virtual Kingdom? SourceKingdom { get; set; }
    public virtual Kingdom? TargetKingdom { get; set; }
}