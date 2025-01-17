﻿//-----------------------------------------------------------------------
// <copyright file="RelationTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Kingdoms;

namespace ROH.Context.Player.TypeConfiguration.Kingdoms;

public class RelationTypeConfiguration : IEntityTypeConfiguration<KingdomRelation>
{
    public void Configure(EntityTypeBuilder<KingdomRelation> builder)
    {
        _ = builder.HasKey(k => k.Id);

        _ = builder.HasOne(k => k.Kingdom).WithMany(k => k.KingdomRelations).HasForeignKey(k => k.IdKingdom);
        _ = builder.Ignore(k => k.Kingdom);

        _ = builder.HasOne(k => k.Kingdom2).WithMany(k => k.KingdomRelations).HasForeignKey(k => k.IdKingdom2);
    }
}