using AutoMapper;

using FluentValidation;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;
public class UserService(IExceptionHandler handler, IValidator<UserModel> userValidator, IUserRepository repository, IMapper mapper) : IUserService
{
    public async Task<DefaultResponse> NewUser(UserModel userModel)
    {
        try
        {
            FluentValidation.Results.ValidationResult validation = await userValidator.ValidateAsync(userModel);
            if (validation != null && !validation.IsValid && validation.Errors.Count > 0)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            if (await repository.EmailInUse(userModel.Email!))
                return new DefaultResponse(httpStatus: HttpStatusCode.Conflict, message: "The email are currently in use.");

            User user = new()
            {
                Email = userModel.Email
            };

            user.SetPassword(userModel.Password!);

            _ = await repository.CreateNewUser(user);

            return new DefaultResponse(httpStatus: HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }

    public async Task<UserModel?> FindUserByEmail(string email) => mapper.Map<UserModel>(await repository.FindUserByEmail(email));

    public async Task<UserModel?> FindUserByUserName(string userName) => mapper.Map<UserModel>(await repository.FindUserByUserName(userName));

    public async Task<UserModel> GetUserByGuid(Guid userGuid) => mapper.Map<UserModel>(await repository.GetUserByGuid(userGuid));

    public async Task<bool> ValidatePassword(string password, Guid userGuid)
    {
        var user = await repository.GetUserByGuid(userGuid);

        return user.VerifyPassword(password);
    }
        
}
