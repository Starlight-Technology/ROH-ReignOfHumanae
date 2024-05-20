using Microsoft.EntityFrameworkCore;

using ROH.Interfaces;
using ROH.Interfaces.Repository.Account;

namespace ROH.Repository.Account;
public class UserRepository(ISqlContext context) : IUserRepository
{
    public Task<bool> EmailInUse(string email) => context.Users.AnyAsync(u => u.Email == email);
}
