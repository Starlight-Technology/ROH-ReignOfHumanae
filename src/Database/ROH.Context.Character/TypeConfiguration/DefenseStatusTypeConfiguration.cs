//-----------------------------------------------------------------------
// <copyright file="DefenseStatusTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Character.Entities;

namespace ROH.Context.Character.TypeConfiguration;

public class DefenseStatusTypeConfiguration : IEntityTypeConfiguration<DefenseStatus>
{
    public void Configure(EntityTypeBuilder<DefenseStatus> builder)
    {
        _ = builder.HasKey(ds => ds.IdCharacter);

        _ = builder.HasOne(a => a.Character)
            .WithOne(c => c.DefenseStatus)
            .HasForeignKey<DefenseStatus>(a => a.IdCharacter);
    }
}