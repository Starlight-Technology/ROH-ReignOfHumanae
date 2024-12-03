//-----------------------------------------------------------------------
// <copyright file="SkillTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ROH.Context.Player.TypeConfiguration.Characters;

public class SkillTypeConfiguration : IEntityTypeConfiguration<Entities.Characters.Skill>
{
    public void Configure(EntityTypeBuilder<Entities.Characters.Skill> builder) => builder.HasKey(s => s.Id);
}