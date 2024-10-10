//-----------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;

public interface IAccountService
{
    Task<DefaultResponse> GetAccountByUserGuid(Guid userGuid);

    Task<DefaultResponse> UpdateAccount(AccountModel accountModel);
}