using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ROH.Domain.Accounts;

namespace ROH.Context.TypeConfiguration.Accounts
{
    public class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            _ = builder.HasKey(a => a.Id);

            _ = builder.HasOne(a => a.User).WithOne(u => u.Account).HasForeignKey<Account>(a => a.IdUser);

            _ = builder.HasMany(a => a.Characters).WithOne(c => c.Account).HasForeignKey(c => c.IdAccount);
        }
    }
}