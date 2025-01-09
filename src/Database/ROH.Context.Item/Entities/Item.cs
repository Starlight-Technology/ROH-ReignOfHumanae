//-----------------------------------------------------------------------
// <copyright file="Item.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Item.Entities;

public record Item(
    long Id,
    Guid Guid,
    int? Attack,
    int? Defense,
    int Weight,
    string? Name,
    string? Descricao,
    string? Sprite,
    string? File,
    string? Format)
{
    public virtual ICollection<ItemEnchantment>? Enchantments { get; set; }
}