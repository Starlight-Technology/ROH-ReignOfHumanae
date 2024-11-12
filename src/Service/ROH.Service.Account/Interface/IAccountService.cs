//-----------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Service.Account.Interface;

public interface IAccountService
{
    Task<DefaultResponse> GetAccountByUserGuidAsync(Guid userGuid, CancellationToken cancellationToken = default);

    Task<DefaultResponse> UpdateAccountAsync(AccountModel accountModel, CancellationToken cancellationToken = default);
}