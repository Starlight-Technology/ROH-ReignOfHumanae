using FluentValidation;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;
public class UserService(IExceptionHandler handler, IValidator<UserModel> userValidator, IUserRepository repository) : IUserService
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

    public async Task<User?> FindUserByEmail(string email) => await repository.FindUserByEmail(email);

    public async Task<User?> FindUserByUserName(string userName) => await repository.FindUserByUserName(userName);

    public async Task<User> GetUserByGuid (Guid userGuid) => await repository.GetUserByGuid(userGuid);
}
