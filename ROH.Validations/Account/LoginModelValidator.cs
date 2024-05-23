using FluentValidation;

using ROH.StandardModels.Account;

namespace ROH.Validations.Account;
public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
