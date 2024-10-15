//-----------------------------------------------------------------------
// <copyright file="ItemTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Items;

namespace ROH.Context.TypeConfiguration.Items;

public class ItemTypeConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        _ = builder.HasKey(i => i.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasMany(i => i.Enchantments).WithOne(e => e.Item).HasForeignKey(i => i.IdItem);
    }
}