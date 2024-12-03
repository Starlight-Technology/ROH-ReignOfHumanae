//-----------------------------------------------------------------------
// <copyright file="GuildTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Guilds;

namespace ROH.Context.Player.TypeConfiguration.Guilds;

public class GuildTypeConfiguration : IEntityTypeConfiguration<Guild>
{
    public void Configure(EntityTypeBuilder<Guild> builder)
    {
        _ = builder.HasKey(g => g.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasMany(g => g.Characters).WithOne(c => c.Guild).HasForeignKey(c => c.IdGuild);
        _ = builder.HasMany(g => g.MembersPositions).WithOne(p => p.Guild).HasForeignKey(p => p.IdGuild);
    }
}