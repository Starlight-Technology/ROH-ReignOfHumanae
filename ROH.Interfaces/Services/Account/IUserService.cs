using ROH.Domain.Accounts;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;
public interface IUserService
{
    Task<DefaultResponse> NewUser(UserModel userModel);
    Task<UserModel?> FindUserByEmail(string email);
    Task<UserModel?> FindUserByUserName(string userName);
    Task<UserModel> GetUserByGuid(Guid userGuid);
    Task<bool> ValidatePassword(string password, Guid userGuid);
}