using FluentValidation;

using ROH.Domain.Version;
using ROH.StandardModels.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Validations.Version
{
    public class GameVersionFileModelValidator : AbstractValidator<GameVersionFileModel>
    {
        public GameVersionFileModelValidator()
        {
            RuleFor(f => f.GameVersion).NotNull().SetValidator(new GameVersionModelValidator() as IValidator<GameVersionModel?>);
            RuleFor(f => f.Content).NotEmpty();
            RuleFor(f => f.Format).NotEmpty();
            RuleFor(f => f.Name).NotEmpty();
            RuleFor(f => f.Size).GreaterThan(0);
        }
    }
}
