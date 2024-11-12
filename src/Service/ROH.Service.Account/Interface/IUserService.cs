//-----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Service.Account.Interface;

public interface IUserService
{
    Task<UserModel?> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<UserModel?> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task<UserModel> GetUserByGuidAsync(Guid userGuid, CancellationToken cancellationToken = default);

    Task<DefaultResponse> NewUserAsync(UserModel userModel, CancellationToken cancellationToken = default);

    Task<bool> ValidatePasswordAsync(string password, Guid userGuid, CancellationToken cancellationToken = default);
}