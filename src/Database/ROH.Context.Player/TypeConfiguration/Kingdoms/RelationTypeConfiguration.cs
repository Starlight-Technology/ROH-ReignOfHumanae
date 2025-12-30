//-----------------------------------------------------------------------
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
        builder.HasKey(k => k.Id);

        builder.HasOne(k => k.SourceKingdom)
            .WithMany(k => k.OutgoingRelations)
            .HasForeignKey(k => k.IdKingdom)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(k => k.TargetKingdom)
            .WithMany(k => k.IncomingRelations)
            .HasForeignKey(k => k.IdKingdom2)
            .OnDelete(DeleteBehavior.Restrict);
    }
}