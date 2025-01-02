//-----------------------------------------------------------------------
// <copyright file="AccountTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ROH.Context.Account.TypeConfiguration;

public class AccountTypeConfiguration : IEntityTypeConfiguration<Entity.Account>
{
    public void Configure(EntityTypeBuilder<Entity.Account> builder)
    {
        _ = builder.HasKey(a => a.Id);

        _ = builder.HasOne(a => a.User).WithOne(u => u.Account).HasForeignKey<Entity.Account>(a => a.IdUser);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");
    }
}