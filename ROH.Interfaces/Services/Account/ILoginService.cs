using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;
public interface ILoginService
{
    Task<DefaultResponse> Login(LoginModel loginModel);
}