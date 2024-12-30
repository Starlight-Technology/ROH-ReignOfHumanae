//-----------------------------------------------------------------------
// <copyright file="HandRing.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Player.Entities.Characters;

public record HandRing(long Id, long IdEquippedItems, long IdItem)
{
    public virtual EquippedItems? EquippedItems { get; set; }
}