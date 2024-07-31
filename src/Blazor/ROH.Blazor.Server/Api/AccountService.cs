using ROH.Blazor.Server.Interfaces.Api;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Blazor.Server.Api;

public class AccountService : IAccountService
{
    private readonly Utils.ApiConfiguration.Gateway _gateway = new();

    public async Task<DefaultResponse?> Login(LoginModel login) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.Login, login);
    public async Task<DefaultResponse?> CreateNewUser(UserModel user) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.CreateNewUser, user);
    public async Task<DefaultResponse?> FindUserByEmail(string email) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.FindUserByEmail, email);
    public async Task<DefaultResponse?> FindUserByUserName(string userName) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.FindUserByUserName, userName);
    public async Task<DefaultResponse?> GetUserByGuid(Guid guid) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.GetUserByGuid, guid);
    public async Task<DefaultResponse?> GetAccountByUserGuid(Guid userGuid) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.GetAccountByUserGuid, userGuid);
    public async Task<DefaultResponse?> UpdateAccount(AccountModel account) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.UpdateAccount, account);

}
