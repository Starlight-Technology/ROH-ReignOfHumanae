using Microsoft.EntityFrameworkCore;

using ROH.Interfaces;
using ROH.Interfaces.Repository.Account;

namespace ROH.Repository.Account;
public class AccountRepository(ISqlContext context) : IAccountRepository
{
    public async Task<Domain.Accounts.Account?> GetAccountById(long id) => await context.Accounts.FindAsync(id);

    public async Task<Domain.Accounts.Account?> GetAccountByGuid(Guid guid) => await context.Accounts.FirstOrDefaultAsync(a => a.Guid == guid);

    public async Task<Domain.Accounts.Account?> GetAccountByUserGuid(Guid guid) 
    {
        var user = await context.Users.FirstOrDefaultAsync(a => a.Guid == guid);

        if (user is null)
            return null;

        return await context.Accounts.FindAsync(user.IdAccount);
    }

    public async Task UpdateAccount(Domain.Accounts.Account account)
    {
        _ = context.Accounts.Update(account);
        _ = await context.SaveChangesAsync();
    }

}
