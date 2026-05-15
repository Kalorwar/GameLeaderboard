using FluentValidation;
using GameLeaderboard.Application.DTOs;

namespace GameLeaderboard.Application.Validations;

public class SubmitScoreRequestValidator : AbstractValidator<SubmitScoreRequest>
{
    public SubmitScoreRequestValidator()
    {
        RuleFor(x => x.Points)
            .GreaterThan(0).WithMessage("Points must be greater than zero.");

        RuleFor(x => x.Level)
            .GreaterThanOrEqualTo(1).WithMessage("Level must be at least 1.");
    }
}