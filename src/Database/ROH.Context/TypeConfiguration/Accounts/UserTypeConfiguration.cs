//-----------------------------------------------------------------------
// <copyright file="UserTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Accounts;

namespace ROH.Context.TypeConfiguration.Accounts;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _ = builder.HasKey(u => u.Id);

        _ = builder.Property(g => g.Guid).HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasOne(u => u.Account).WithOne(a => a.User).HasForeignKey<User>(u => u.IdAccount);
    }
}