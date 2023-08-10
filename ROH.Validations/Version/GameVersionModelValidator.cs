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
    public class GameVersionModelValidator : AbstractValidator<GameVersionModel>
    {
        public GameVersionModelValidator()
        {
            RuleFor(g => g.Version).GreaterThanOrEqualTo(0);
            RuleFor(g => g.Release).GreaterThanOrEqualTo(0);
            RuleFor(g => g.Review).GreaterThanOrEqualTo(0);
        }
    }
}
