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
    public async Task<UserModel?> FindUserByEmail(string email) => mapper.Map<UserModel>(
        await repository.FindUserByEmailAsync(email));

    public async Task<UserModel?> FindUserByUserName(string userName) => mapper.Map<UserModel>(
        await repository.FindUserByUserNameAsync(userName));

    public async Task<UserModel> GetUserByGuid(Guid userGuid) => mapper.Map<UserModel>(
        await repository.GetUserByGuidAsync(userGuid));

    public async Task<DefaultResponse> NewUser(UserModel userModel)
    {
        try
        {
            ValidationResult validation = await userValidator.ValidateAsync(userModel);
            if ((validation != null) && !validation.IsValid && (validation.Errors.Count > 0))
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            if (await repository.EmailInUseAsync(userModel.Email!))
                return new DefaultResponse(
                    httpStatus: HttpStatusCode.Conflict,
                    message: "The email are currently in use.");

            User user = new() { Email = userModel.Email, UserName = userModel.UserName };

            user.SetPassword(userModel.Password!);

            _ = await repository.CreateNewUserAsync(user);

            return new DefaultResponse(httpStatus: HttpStatusCode.OK, message: "Account has been created!");
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }

    public async Task<bool> ValidatePassword(string password, Guid userGuid)
    {
        User user = await repository.GetUserByGuidAsync(userGuid);

        return user.VerifyPassword(password);
    }
}
