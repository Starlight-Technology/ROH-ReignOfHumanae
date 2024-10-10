//-----------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Blazor.Server.Interfaces.Api;

public interface IAccountService
{
    Task<DefaultResponse?> CreateNewUser(UserModel user);

    Task<DefaultResponse?> FindUserByEmail(string email);

    Task<DefaultResponse?> FindUserByUserName(string userName);

    Task<DefaultResponse?> GetAccountByUserGuid(Guid userGuid);

    Task<DefaultResponse?> GetUserByGuid(Guid guid);

    Task<DefaultResponse?> Login(LoginModel login);

    Task<DefaultResponse?> UpdateAccount(AccountModel account);
}
