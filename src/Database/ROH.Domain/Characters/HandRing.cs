//-----------------------------------------------------------------------
// <copyright file="HandRing.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using ROH.Domain.Items;

namespace ROH.Domain.Characters;

public record HandRing(long Id, long IdEquippedItems, long IdItem)
{
    public virtual EquippedItems? EquippedItems { get; set; }

    public virtual Item? Item { get; set; }
}