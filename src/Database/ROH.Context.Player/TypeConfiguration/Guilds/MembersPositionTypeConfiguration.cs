//-----------------------------------------------------------------------
// <copyright file="MembersPositionTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Guilds;

namespace ROH.Context.Player.TypeConfiguration.Guilds;

public class MembersPositionTypeConfiguration : IEntityTypeConfiguration<MembersPosition>
{
    public void Configure(EntityTypeBuilder<MembersPosition> builder)
    {
        _ = builder.HasKey(p => p.Id);

        _ = builder.HasOne(p => p.Guild).WithMany(g => g.MembersPositions).HasForeignKey(p => p.IdGuild);
        _ = builder.HasOne(p => p.Character).WithOne().HasForeignKey<MembersPosition>(p => p.IdCharacter);
    }
}