using Microsoft.EntityFrameworkCore;

using ROH.Interfaces;
using ROH.Interfaces.Repository.Account;

namespace ROH.Repository.Account;

public class AccountRepository(ISqlContext context) : IAccountRepository
{
    public async Task<Domain.Accounts.Account?> GetAccountById(long id) => await context.Accounts.FindAsync(id);

    public async Task<Domain.Accounts.Account?> GetAccountByGuid(Guid guid) => await context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Guid == guid);

    public async Task<Domain.Accounts.Account?> GetAccountByUserGuid(Guid guid)
    {
        Domain.Accounts.User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Guid == guid);

        return user is null ? null : await context.Accounts.AsNoTracking().FirstAsync(a => a.Id == user.IdAccount);
    }

    public async Task UpdateAccount(Domain.Accounts.Account account)
    {
        _ = context.Accounts.Update(account);
        _ = await context.SaveChangesAsync();
    }
}
