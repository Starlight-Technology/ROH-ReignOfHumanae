using FluentValidation;

using ROH.Domain.Accounts;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;
public class LoginService(IExceptionHandler handler, IValidator<LoginModel> loginValidator, IUserService userService)
{
    public async Task<DefaultResponse> Login(LoginModel loginModel)
    {
        try
        {
            FluentValidation.Results.ValidationResult validation = await loginValidator.ValidateAsync(loginModel);
            if (validation != null && !validation.IsValid && validation.Errors.Count > 0)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            User? user = await FindUser(loginModel);

            if (user is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "User not found.");

            return new DefaultResponse(objectResponse: new UserModel() { Email = user.Email, UserName = user.UserName, Guid = user.Guid });
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }

    private async Task<User?> FindUser(LoginModel loginModel) => await userService.FindUserByEmail(loginModel.Login!) ?? await userService.FindUserByUserName(loginModel.Login!);
}
