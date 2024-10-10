//-----------------------------------------------------------------------
// <copyright file="GameVersionTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Version;

namespace ROH.Context.TypeConfiguration.Version;

public class GameVersionTypeConfiguration : IEntityTypeConfiguration<GameVersion>
{
    public void Configure(EntityTypeBuilder<GameVersion> builder)
    {
        _ = builder.HasKey(g => g.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");
    }
}