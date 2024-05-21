using FluentValidation;

using ROH.StandardModels.Account;

namespace ROH.Validations.Account;
public class UserModelValidation : AbstractValidator<UserModel>
{
    public UserModelValidation()
    {
        _ = RuleFor(r => r.Email).NotEmpty().NotNull().EmailAddress();
        _ = RuleFor(r => r.Password).NotEmpty().NotNull();
    }
}
