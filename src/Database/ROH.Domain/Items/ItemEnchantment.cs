//-----------------------------------------------------------------------
// <copyright file="ItemEnchantment.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Domain.Items;

public record ItemEnchantment(long Id, long IdItem, long IdEnchantment)
{
    public virtual Enchantment? Enchantment { get; set; }

    public virtual Item? Item { get; set; }
}