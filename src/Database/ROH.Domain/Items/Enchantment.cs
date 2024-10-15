//-----------------------------------------------------------------------
// <copyright file="Enchantments.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Effect;

namespace ROH.Domain.Items;

public record Enchantment(long Id, long? Damage, long? Defense, string? Animation, string Name, EffectType Type)
{
    public virtual ICollection<ItemEnchantment>? Items { get; set; }
}