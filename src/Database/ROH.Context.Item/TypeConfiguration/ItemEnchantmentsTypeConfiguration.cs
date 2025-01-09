//-----------------------------------------------------------------------
// <copyright file="ItemEnchantmentsTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Item.Entities;

namespace ROH.Context.Item.TypeConfiguration;

public class ItemEnchantmentsTypeConfiguration : IEntityTypeConfiguration<ItemEnchantment>
{
    public void Configure(EntityTypeBuilder<ItemEnchantment> builder)
    {
        _ = builder.HasKey(ie => ie.Id);

        _ = builder.HasOne(ie => ie.Item).WithMany(i => i.Enchantments).HasForeignKey(ie => ie.IdItem);
        _ = builder.HasOne(ie => ie.Enchantment).WithMany(i => i.Items).HasForeignKey(ie => ie.IdEnchantment);
    }
}