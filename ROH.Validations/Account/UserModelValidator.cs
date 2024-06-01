using FluentValidation;

using ROH.StandardModels.Account;

namespace ROH.Validations.Account;

public class UserModelValidator : AbstractValidator<UserModel>
{
    public UserModelValidator()
    {
        _ = RuleFor(r => r.Email).NotEmpty().NotNull().EmailAddress();
        _ = RuleFor(r => r.Password).NotEmpty().NotNull();
    }
}
