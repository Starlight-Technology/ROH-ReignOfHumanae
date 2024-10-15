//-----------------------------------------------------------------------
// <copyright file="EnchantmentsTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Items;

namespace ROH.Context.TypeConfiguration.Items;

public class EnchantmentsTypeConfiguration : IEntityTypeConfiguration<Enchantment>
{
    public void Configure(EntityTypeBuilder<Enchantment> builder)
    {
        _ = builder.HasKey(e => e.Id);

        _ = builder.HasMany(e => e.Items).WithOne(i => i.Enchantment).HasForeignKey(e => e.IdEnchantment);
    }
}