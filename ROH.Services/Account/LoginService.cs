using FluentValidation;

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

            UserModel? user = await FindUser(loginModel);

            return user is null
                ? new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "User not found.")
                : await ValidatePassword(user, loginModel);
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }

    private async Task<DefaultResponse> ValidatePassword(UserModel user, LoginModel loginModel) =>
         await userService.ValidatePassword(loginModel.Password!, user.Guid!.Value)
        ? new DefaultResponse(objectResponse: new UserModel() { Email = user.Email, UserName = user.UserName, Guid = user.Guid })
        : new DefaultResponse(httpStatus: HttpStatusCode.Unauthorized, message: "Invalid password!");

    private async Task<UserModel?> FindUser(LoginModel loginModel) => await userService.FindUserByEmail(loginModel.Login!) ?? await userService.FindUserByUserName(loginModel.Login!);
}
