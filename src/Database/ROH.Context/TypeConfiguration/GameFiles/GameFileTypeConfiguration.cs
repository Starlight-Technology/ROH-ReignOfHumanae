//-----------------------------------------------------------------------
// <copyright file="GameFileTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.GameFiles;

namespace ROH.Context.TypeConfiguration.GameFiles;

public class GameFileTypeConfiguration : IEntityTypeConfiguration<GameFile>
{
    public void Configure(EntityTypeBuilder<GameFile> builder)
    {
        _ = builder.HasKey(f => f.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");
    }
}
