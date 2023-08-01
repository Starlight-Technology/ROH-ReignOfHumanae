using FluentValidation;

using ROH.Domain.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Validations.Version
{
    public class GameVersionFileValidator : AbstractValidator<GameVersionFile>
    {
        public GameVersionFileValidator()
        {
            RuleFor(f => f.IdVersion).GreaterThan(0);
            RuleFor(f => f.GameVersion).NotNull().SetValidator(new GameVersionValidator() as IValidator<GameVersion?>);
            RuleFor(f => f.Content).NotEmpty();
            RuleFor(f => f.Format).NotEmpty();
            RuleFor(f => f.Name).NotEmpty();
            RuleFor(f => f.Size).GreaterThan(0);
        }
    }
}
