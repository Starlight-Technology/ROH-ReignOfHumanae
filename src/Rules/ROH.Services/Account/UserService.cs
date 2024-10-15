//-----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;

public class UserService(
    IExceptionHandler handler,
    IValidator<UserModel> userValidator,
    IUserRepository repository,
    IMapper mapper) : IUserService
{
    public async Task<UserModel?> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default) => mapper.Map<UserModel>(
        await repository.FindUserByEmailAsync(email, cancellationToken).ConfigureAwait(true));

    public async Task<UserModel?> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken = default) => mapper.Map<UserModel>(
        await repository.FindUserByUserNameAsync(userName, cancellationToken).ConfigureAwait(true));

    public async Task<UserModel> GetUserByGuidAsync(Guid userGuid, CancellationToken cancellationToken = default) => mapper.Map<UserModel>(
        await repository.GetUserByGuidAsync(userGuid, cancellationToken).ConfigureAwait(true));

    public async Task<DefaultResponse> NewUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        try
        {
            ValidationResult validation = await userValidator.ValidateAsync(userModel, cancellationToken).ConfigureAwait(true);
            if ((validation != null) && !validation.IsValid && (validation.Errors.Count > 0))
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            if (await repository.EmailInUseAsync(userModel.Email!, cancellationToken).ConfigureAwait(true))
                return new DefaultResponse(
                    httpStatus: HttpStatusCode.Conflict,
                    message: "The email are currently in use.");

            User user = new() { Email = userModel.Email, UserName = userModel.UserName };

            user.SetPassword(userModel.Password!);

            _ = await repository.CreateNewUserAsync(user, cancellationToken).ConfigureAwait(true);

            return new DefaultResponse(httpStatus: HttpStatusCode.OK, message: "Account has been created!");
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }

    public async Task<bool> ValidatePasswordAsync(string password, Guid userGuid, CancellationToken cancellationToken = default)
    {
        User user = await repository.GetUserByGuidAsync(userGuid, cancellationToken).ConfigureAwait(true);

        return user.VerifyPassword(password);
    }
}
