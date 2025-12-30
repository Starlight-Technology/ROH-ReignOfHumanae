//-----------------------------------------------------------------------
// <copyright file="CharacterTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.TypeConfiguration.Characters;

public class CharacterTypeConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        _ = builder.HasKey(c => c.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasOne(c => c.AttackStatus)
            .WithOne(a => a.Character)
            .HasForeignKey<AttackStatus>(a => a.IdCharacter)
            .IsRequired(false);

        _ = builder.HasOne(c => c.DefenseStatus)
            .WithOne(a => a.Character)
            .HasForeignKey<DefenseStatus>(a => a.IdCharacter)
            .IsRequired(false);

        _ = builder.HasOne(c => c.Status)
            .WithOne(a => a.Character)
            .HasForeignKey<Status>(a => a.IdCharacter)
            .IsRequired(false);

        _ = builder.HasOne(c => c.EquippedItems)
            .WithOne(a => a.Character)
            .HasForeignKey<EquippedItems>(a => a.IdCharacter)
            .IsRequired(false);

        builder.HasOne(p => p.PlayerPosition)
            .WithOne(p => p.Player)
            .HasForeignKey<PlayerPosition>(p => p.IdPlayer)
            .IsRequired(false);

        _ = builder.HasMany(c => c.Skills).WithOne(s => s.Character).HasForeignKey(c => c.IdCharacter).IsRequired(false);

        _ = builder.HasMany(c => c.Inventory)
            .WithOne(i => i.Character)
            .HasForeignKey(i => i.IdCharacter)
            .IsRequired(false);

        builder.HasOne(c => c.Guild).WithMany().HasForeignKey(c => c.IdGuild).IsRequired(false);

        builder.HasOne(c => c.Kingdom).WithMany().HasForeignKey(c => c.IdKingdom).IsRequired(false);

        builder.HasOne(c => c.Status).WithOne().HasForeignKey<Status>(s => s.IdCharacter).IsRequired(false);
    }
}