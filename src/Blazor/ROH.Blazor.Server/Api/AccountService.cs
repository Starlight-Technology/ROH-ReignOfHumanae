//-----------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;
using ROH.Utils.ApiConfiguration;

namespace ROH.Blazor.Server.Api;

public class AccountService(ICustomAuthenticationStateProvider customAuthenticationStateProvider) : IAccountService
{
    private readonly Gateway _gateway = new();

    public async Task<DefaultResponse?> CreateNewUser(UserModel user) => await _gateway.PostAsync(
        Gateway.Services.CreateNewUser,
        user,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> FindUserByEmail(string email) => await _gateway.PostAsync(
        Gateway.Services.FindUserByEmail,
        email,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> FindUserByUserName(string userName) => await _gateway.PostAsync(
        Gateway.Services.FindUserByUserName,
        userName,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAccountByUserGuid(Guid userGuid) => await _gateway.PostAsync(
        Gateway.Services.GetAccountByUserGuid,
        userGuid,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetUserByGuid(Guid guid) => await _gateway.PostAsync(
        Gateway.Services.GetUserByGuid,
        guid,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> Login(LoginModel login) => await _gateway.PostAsync(
        Gateway.Services.Login,
        login,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> UpdateAccount(AccountModel account) => await _gateway.PostAsync(
        Gateway.Services.UpdateAccount,
        account,
        await customAuthenticationStateProvider.GetToken());
}
