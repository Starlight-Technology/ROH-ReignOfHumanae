using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;
public interface IUserService
{
    Task<DefaultResponse> NewUser(UserModel userModel);
}