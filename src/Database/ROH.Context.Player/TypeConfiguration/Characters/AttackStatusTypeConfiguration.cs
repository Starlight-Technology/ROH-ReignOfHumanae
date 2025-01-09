//-----------------------------------------------------------------------
// <copyright file="AttackStatusTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.TypeConfiguration.Characters;

public class AttackStatusTypeConfiguration : IEntityTypeConfiguration<Entities.Characters.AttackStatus>
{
    public void Configure(EntityTypeBuilder<AttackStatus> builder)
    {
        _ = builder.HasKey(a => a.IdCharacter);

        _ = builder.HasOne(a => a.Character)
            .WithOne(c => c.AttackStatus)
            .HasForeignKey<AttackStatus>(a => a.IdCharacter);
    }
}