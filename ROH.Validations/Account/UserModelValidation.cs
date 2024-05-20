using FluentValidation;

using ROH.StandardModels.Account;

namespace ROH.Validations.Account;
public class UserModelValidation : AbstractValidator<UserModel>
{
    public UserModelValidation()
    {
        RuleFor(r => r.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(r => r.Password).NotEmpty().NotNull();
    }
}
