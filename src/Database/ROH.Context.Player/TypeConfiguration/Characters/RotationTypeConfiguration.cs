//-----------------------------------------------------------------------
// <copyright file="RotationTypeConfiguration.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.TypeConfiguration.Characters;

class RotationTypeConfiguration : IEntityTypeConfiguration<Rotation>
{
    public void Configure(EntityTypeBuilder<Rotation> builder) => builder.HasKey(p => p.Id);
}