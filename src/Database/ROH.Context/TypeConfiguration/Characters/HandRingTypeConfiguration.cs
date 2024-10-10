//-----------------------------------------------------------------------
// <copyright file="HandRingTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Characters;

namespace ROH.Context.TypeConfiguration.Characters;

public class HandRingTypeConfiguration : IEntityTypeConfiguration<HandRing>
{
    public void Configure(EntityTypeBuilder<HandRing> builder)
    {
        _ = builder.HasKey(r => r.Id);

        _ = builder.HasOne(r => r.Item).WithMany().HasForeignKey(r => r.IdItem);
    }
}