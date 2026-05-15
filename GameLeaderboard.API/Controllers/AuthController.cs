using FluentValidation;
using GameLeaderboard.Application.DTOs;
using GameLeaderboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameLeaderboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<AuthRequest> _validator;

    public AuthController(IAuthService authService, IValidator<AuthRequest> validator)
    {
        _authService = authService;
        _validator = validator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = await _authService.LoginOrRegisterAsync(request, cancellationToken);
        return Ok(response);
    }
}