//-----------------------------------------------------------------------
// <copyright file="CharacterSkillTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Character.Entities;

namespace ROH.Context.Character.TypeConfiguration;

public record CharacterSkillTypeConfiguration : IEntityTypeConfiguration<CharacterSkill>
{
    public void Configure(EntityTypeBuilder<CharacterSkill> builder)
    {
        _ = builder.HasKey(cs => cs.Id);

        _ = builder.HasOne(cs => cs.Character).WithMany(c => c.Skills).HasForeignKey(cs => cs.IdCharacter);
        _ = builder.HasOne(cs => cs.Skill).WithMany().HasForeignKey(cs => cs.IdSkill);
    }
}