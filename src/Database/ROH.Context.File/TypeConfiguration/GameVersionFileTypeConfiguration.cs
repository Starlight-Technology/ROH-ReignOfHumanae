//-----------------------------------------------------------------------
// <copyright file="GameVersionFileTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.File.Entities;

namespace ROH.Context.File.TypeConfiguration;

public class GameVersionFileTypeConfiguration : IEntityTypeConfiguration<GameVersionFile>
{
    public void Configure(EntityTypeBuilder<GameVersionFile> builder)
    {
        _ = builder.HasKey(f => f.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasOne(f => f.GameFile).WithOne().HasForeignKey<GameVersionFile>(f => f.IdGameFile);
    }
}