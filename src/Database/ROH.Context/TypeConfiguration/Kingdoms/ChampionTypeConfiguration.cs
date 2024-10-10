//-----------------------------------------------------------------------
// <copyright file="ChampionTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Kingdoms;

namespace ROH.Context.TypeConfiguration.Kingdoms;

public class ChampionTypeConfiguration : IEntityTypeConfiguration<Champion>
{
    public void Configure(EntityTypeBuilder<Champion> builder)
    {
        _ = builder.HasKey(c => c.Id);

        _ = builder.HasOne(c => c.Character).WithMany().HasForeignKey(c => c.IdCharacter);
        _ = builder.HasOne(c => c.Kingdom).WithMany(k => k.Champions).HasForeignKey(c => c.IdKingdom);
    }
}