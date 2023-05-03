using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Accounts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Context.TypeConfiguration.Accounts
{
    public class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.User).WithOne(u => u.Account).HasForeignKey<Account>(a => a.IdUser);

            builder.HasMany(a => a.Characters).WithOne(c => c.Account).HasForeignKey(c => c.IdAccount);
        }
    }
}
