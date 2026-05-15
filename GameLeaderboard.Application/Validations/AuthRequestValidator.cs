using FluentValidation;
using GameLeaderboard.Application.DTOs;

namespace GameLeaderboard.Application.Validations;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("DeviceId is required")
            .MaximumLength(100).WithMessage("DeviceId is too long");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("\"Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username is too long")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");
    }
}