using ROH.StandardModels.Account;

namespace ROH.Interfaces.Authentication;

public interface IAuthService
{
    string GenerateJwtToken(UserModel user);
}