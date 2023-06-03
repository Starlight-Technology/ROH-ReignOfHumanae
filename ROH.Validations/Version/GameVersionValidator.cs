using FluentValidation;

using ROH.Domain.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Validations.Version
{
    public class GameVersionValidator : AbstractValidator<GameVersion>
    {
        public GameVersionValidator()
        {
            RuleFor(g => g.Version).GreaterThanOrEqualTo(0);
            RuleFor(g => g.Release).GreaterThanOrEqualTo(0);
            RuleFor(g => g.Review).GreaterThanOrEqualTo(0);
        }
    }
}
