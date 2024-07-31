using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Blazor.Server.Interfaces.Api;

public interface IAccountService
{
    Task<DefaultResponse?> Login(LoginModel login);
    Task<DefaultResponse?> CreateNewUser(UserModel user);
    Task<DefaultResponse?> FindUserByEmail(string email);
    Task<DefaultResponse?> FindUserByUserName(string userName);
    Task<DefaultResponse?> GetUserByGuid(Guid guid);
    Task<DefaultResponse?> GetAccountByUserGuid(Guid userGuid);
    Task<DefaultResponse?> UpdateAccount(AccountModel account);
}
