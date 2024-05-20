using AutoMapper;

using FluentValidation;

using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;
public class UserService(IExceptionHandler handler, IValidator<UserModel> userValidator, IMapper mapper, IUserRepository repository)
{
    public async Task<DefaultResponse> NewUser(UserModel userModel)
    {
        try
        {
            FluentValidation.Results.ValidationResult validation = await userValidator.ValidateAsync(userModel);
            if (validation != null && !validation.IsValid && validation.Errors.Count > 0)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            if(await repository.EmailInUse(userModel.Email!))
                return new DefaultResponse(httpStatus:HttpStatusCode.Conflict, message: "The email are currently in use.");

            return new DefaultResponse(httpStatus: HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return handler.HandleException(e);
        }
    }
}
