//-----------------------------------------------------------------------
// <copyright file="LoginService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;

using ROH.Service.Account.Interface;
using ROH.Service.Authentication.Interface;
using ROH.Service.Exception.Interface;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Service.Account;

public class LoginService(
    IExceptionHandler handler,
    IValidator<LoginModel> loginValidator,
    IUserService userService,
    IAuthService authService) : ILoginService
{
    async Task<UserModel?> FindUserAsync(LoginModel loginModel, CancellationToken cancellationToken = default) => await userService.FindUserByEmailAsync(
            loginModel.Login!,
            cancellationToken)
            .ConfigureAwait(true) ??
        await userService.FindUserByUserNameAsync(loginModel.Login!, cancellationToken).ConfigureAwait(true);

    async Task<DefaultResponse> ValidatePasswordAsync(
        UserModel user,
        LoginModel loginModel,
        CancellationToken cancellationToken = default) => (await userService.ValidatePasswordAsync(
            loginModel.Password!,
            user.Guid!.Value,
            cancellationToken)
            .ConfigureAwait(true))
        ? (new DefaultResponse(
            objectResponse: new UserModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Guid = user.Guid,
                Token = authService.GenerateJwtToken(user)
            }))
        : (new DefaultResponse(httpStatus: HttpStatusCode.Unauthorized, message: "Invalid password!"));

    public async Task<DefaultResponse> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken = default)
    {
        try
        {
            ValidationResult validation = await loginValidator.ValidateAsync(loginModel, cancellationToken)
                .ConfigureAwait(true);
            if ((validation is not null) && !validation.IsValid && (validation.Errors.Count > 0))
            {
                string errorMessages = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
                return new DefaultResponse(null, HttpStatusCode.BadRequest, errorMessages);
            }

            UserModel? user = await FindUserAsync(loginModel, cancellationToken).ConfigureAwait(true);

            return (user is null)
                ? (new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "User not found."))
                : (await ValidatePasswordAsync(user, loginModel, cancellationToken).ConfigureAwait(true));
        }
        catch (System.Exception e)
        {
            return handler.HandleException(e);
        }
    }
}