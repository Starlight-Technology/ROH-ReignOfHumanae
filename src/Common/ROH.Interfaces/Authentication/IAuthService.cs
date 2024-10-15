//-----------------------------------------------------------------------
// <copyright file="IAuthService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Account;

namespace ROH.Interfaces.Authentication;

public interface IAuthService
{
    string GenerateJwtToken(UserModel user);
}