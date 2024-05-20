using FluentValidation;

using ROH.StandardModels.Version;

namespace ROH.Validations.Version;

public class GameVersionFileModelValidation : AbstractValidator<GameVersionFileModel>
{
    public GameVersionFileModelValidation()
    {
        _ = RuleFor(f => f.GameVersion).NotNull().SetValidator(new GameVersionModelValidation() as IValidator<GameVersionModel?>);
        _ = RuleFor(f => f.Content).NotEmpty();
        _ = RuleFor(f => f.Format).NotEmpty();
        _ = RuleFor(f => f.Name).NotEmpty();
        _ = RuleFor(f => f.Size).GreaterThan(0);
    }
}