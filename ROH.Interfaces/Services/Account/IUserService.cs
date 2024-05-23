using ROH.Domain.Accounts;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;
public interface IUserService
{
    Task<DefaultResponse> NewUser(UserModel userModel);
    Task<User?> FindUserByEmail(string email);
    Task<User?> FindUserByUserName(string userName);
}