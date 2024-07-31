﻿using FluentValidation;

using ROH.StandardModels.Version;

namespace ROH.Validations.Version;

public class GameVersionFileModelValidator : AbstractValidator<GameVersionFileModel>
{
    public GameVersionFileModelValidator()
    {
        _ = RuleFor(f => f.GameVersion).NotNull().SetValidator(new GameVersionModelValidator() as IValidator<GameVersionModel?>);
        _ = RuleFor(f => f.Content).NotEmpty();
        _ = RuleFor(f => f.Format).NotEmpty();
        _ = RuleFor(f => f.Name).NotEmpty();
        _ = RuleFor(f => f.Size).GreaterThan(0);
    }
}