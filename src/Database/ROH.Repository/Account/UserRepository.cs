using Microsoft.EntityFrameworkCore;

using ROH.Domain.Accounts;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Account;

namespace ROH.Repository.Account;

public class UserRepository(ISqlContext context) : IUserRepository
{
    public Task<bool> EmailInUse(string email) => context.Users.AnyAsync(u => u.Email == email);

    public async Task<User> CreateNewUser(User user)
    {
        _ = await context.Users.AddAsync(user);
        _ = await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> FindUserByEmail(string email) => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> FindUserByUserName(string userName) => await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

    public async Task<User> GetUserByGuid(Guid userGuid) => await context.Users.FirstAsync(u => u.Guid == userGuid);
}
