using FluentValidation;

using ROH.StandardModels.Version;

namespace ROH.Validations.Version;

public class GameVersionModelValidator : AbstractValidator<GameVersionModel>
{
    public GameVersionModelValidator()
    {
        _ = RuleFor(g => g.Version).GreaterThanOrEqualTo(0);
        _ = RuleFor(g => g.Release).GreaterThanOrEqualTo(0);
        _ = RuleFor(g => g.Review).GreaterThanOrEqualTo(0);
    }
}