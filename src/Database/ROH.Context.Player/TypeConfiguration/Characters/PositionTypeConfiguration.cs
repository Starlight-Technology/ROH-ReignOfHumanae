//-----------------------------------------------------------------------
// <copyright file="PositionTypeConfiguration.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.TypeConfiguration.Characters;

class PositionTypeConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder) => builder.HasKey(p => p.Id);
}