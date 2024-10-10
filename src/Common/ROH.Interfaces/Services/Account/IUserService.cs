//-----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;

public interface IUserService
{
    Task<UserModel?> FindUserByEmail(string email);

    Task<UserModel?> FindUserByUserName(string userName);

    Task<UserModel> GetUserByGuid(Guid userGuid);

    Task<DefaultResponse> NewUser(UserModel userModel);

    Task<bool> ValidatePassword(string password, Guid userGuid);
}