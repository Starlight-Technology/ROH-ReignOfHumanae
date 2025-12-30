//-----------------------------------------------------------------------
// <copyright file="LoginModelValidator.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using FluentValidation;

using ROH.StandardModels.Account;

namespace ROH.Validations.Account;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        _ = RuleFor(x => x.Login).NotEmpty();
        _ = RuleFor(x => x.Password).NotEmpty();
    }
}