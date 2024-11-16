//-----------------------------------------------------------------------
// <copyright file="EquippedItemsTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Character.Entities;

namespace ROH.Context.Character.TypeConfiguration;

public class EquippedItemsTypeConfiguration : IEntityTypeConfiguration<EquippedItems>
{
    public void Configure(EntityTypeBuilder<EquippedItems> builder)
    {
        _ = builder.HasKey(e => e.IdCharacter);

        _ = builder.HasOne(e => e.Character)
            .WithOne(c => c.EquippedItems)
            .HasForeignKey<EquippedItems>(e => e.IdCharacter);

        _ = builder.HasMany(e => e.LeftHandRings).WithOne(l => l.EquippedItems).HasForeignKey(l => l.IdEquippedItems);
        _ = builder.Ignore(e => e.LeftHandRings);
        _ = builder.HasMany(e => e.RightHandRings).WithOne(l => l.EquippedItems).HasForeignKey(l => l.IdEquippedItems);
    }
}