using FluentValidation;

using ROH.StandardModels.Version;

namespace ROH.Validations.Version;

public class GameVersionModelValidation : AbstractValidator<GameVersionModel>
{
    public GameVersionModelValidation()
    {
        _ = RuleFor(g => g.Version).GreaterThanOrEqualTo(0);
        _ = RuleFor(g => g.Release).GreaterThanOrEqualTo(0);
        _ = RuleFor(g => g.Review).GreaterThanOrEqualTo(0);
    }
}