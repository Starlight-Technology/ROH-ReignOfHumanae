//-----------------------------------------------------------------------
// <copyright file="UserModelValidator.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using FluentValidation;

using ROH.StandardModels.Account;

namespace ROH.Validations.Account;

public class UserModelValidator : AbstractValidator<UserModel>
{
    public UserModelValidator()
    {
        _ = RuleFor(r => r.Email).NotEmpty().NotNull().EmailAddress();
        _ = RuleFor(r => r.Password).NotEmpty().NotNull();
    }
}