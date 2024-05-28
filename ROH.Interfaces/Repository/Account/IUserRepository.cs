using ROH.Domain.Accounts;

namespace ROH.Interfaces.Repository.Account;

public interface IUserRepository
{
    Task<bool> EmailInUse(string email);
    Task<User> CreateNewUser(User user);
    Task<User?> FindUserByEmail(string email);
    Task<User?> FindUserByUserName(string userName);
    Task<User> GetUserByGuid(Guid userGuid);
}