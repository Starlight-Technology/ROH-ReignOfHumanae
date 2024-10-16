﻿//-----------------------------------------------------------------------
// <copyright file="AccountTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Accounts;

namespace ROH.Context.TypeConfiguration.Accounts;

public class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        _ = builder.HasKey(a => a.Id);

        _ = builder.HasOne(a => a.User).WithOne(u => u.Account).HasForeignKey<Account>(a => a.IdUser);

        _ = builder.HasMany(a => a.Characters).WithOne(c => c.Account).HasForeignKey(c => c.IdAccount);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");
    }
}