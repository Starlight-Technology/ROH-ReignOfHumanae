using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Blazor.Server.Api;

public class AccountService(ICustomAuthenticationStateProvider customAuthenticationStateProvider) : IAccountService
{
    private readonly Utils.ApiConfiguration.Gateway _gateway = new();

    public async Task<DefaultResponse?> Login(LoginModel login) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.Login, login, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> CreateNewUser(UserModel user) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.CreateNewUser, user, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> FindUserByEmail(string email) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.FindUserByEmail, email, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> FindUserByUserName(string userName) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.FindUserByUserName, userName, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetUserByGuid(Guid guid) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.GetUserByGuid, guid, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAccountByUserGuid(Guid userGuid) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.GetAccountByUserGuid, userGuid, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> UpdateAccount(AccountModel account) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.UpdateAccount, account, await customAuthenticationStateProvider.GetToken());
}
