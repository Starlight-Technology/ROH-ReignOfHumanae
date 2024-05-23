using FluentValidation;

using ROH.Domain.Accounts;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;
public class LoginService(IExceptionHandler handler, IValidator<LoginModel> loginValidator, IUserService userService) : ILoginService
{
    public async Task<DefaultResponse> Login(LoginModel loginModel)
    {
        try
        {
            FluentValidation.Results.ValidationResult validation = await loginValidator.ValidateAsync(loginModel);
            if (validation != null && !validation.IsValid && validation.Errors.Count > 0)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            User? user = await FindUser(loginModel);

            return user is null
                ? new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "User not found.")
                : ValidatePassword(user, loginModel);
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }

    private static DefaultResponse ValidatePassword(User user, LoginModel loginModel) => user.VerifyPassword(loginModel.Password!)
        ? new DefaultResponse(objectResponse: new UserModel() { Email = user.Email, UserName = user.UserName, Guid = user.Guid })
        : new DefaultResponse(httpStatus: HttpStatusCode.Unauthorized, message: "Invalid password!");

    private async Task<User?> FindUser(LoginModel loginModel) => await userService.FindUserByEmail(loginModel.Login!) ?? await userService.FindUserByUserName(loginModel.Login!);
}
