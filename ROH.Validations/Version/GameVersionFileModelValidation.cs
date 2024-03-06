using FluentValidation;

using ROH.StandardModels.Version;

namespace ROH.Validations.Version
{
    public class GameVersionFileModelValidation : AbstractValidator<GameVersionFileModel>
    {
        public GameVersionFileModelValidation()
        {
             RuleFor(f => f.GameVersion).NotNull().SetValidator(new GameVersionModelValidation() as IValidator<GameVersionModel?>);
             RuleFor(f => f.Content).NotEmpty();
             RuleFor(f => f.Format).NotEmpty();
             RuleFor(f => f.Name).NotEmpty();
             RuleFor(f => f.Size).GreaterThan(0);
        }
    }
}