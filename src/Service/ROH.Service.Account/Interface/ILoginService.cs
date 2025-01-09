//-----------------------------------------------------------------------
// <copyright file="ILoginService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Service.Account.Interface;

public interface ILoginService
{
    Task<DefaultResponse> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken = default);
}