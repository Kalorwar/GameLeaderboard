using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using GameLeaderboard.Application.DTOs;
using GameLeaderboard.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLeaderboard.API.Controllers;

[ApiController]
[Authorize]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    private readonly IValidator<SubmitScoreRequest> _scoreValidator;

    public LeaderboardController(ILeaderboardService leaderboardService, IValidator<SubmitScoreRequest> scoreValidator)
    {
        _leaderboardService = leaderboardService;
        _scoreValidator = scoreValidator;
    }

    private Guid GetCurrentUserId()
    {
        var idClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.Parse(idClaim!);
    }

    [HttpPost("api/scores")]
    public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _scoreValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = GetCurrentUserId();
        await _leaderboardService.SubmitScoreAsync(userId, request, cancellationToken);

        return Ok(new { Message = "Score submitted successfully." });
    }

    [HttpGet("api/scores/me")]
    public async Task<IActionResult> GetMyScores(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var scores = await _leaderboardService.GetUserRecentScoresAsync(userId, cancellationToken);
        return Ok(scores);
    }

    [AllowAnonymous]
    [HttpGet("api/leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int top = 10, [FromQuery] int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var leaderboard = await _leaderboardService.GetTopPlayersAsync(top, offset, cancellationToken);
        return Ok(leaderboard);
    }

    [HttpGet("api/leaderboard/me")]
    public async Task<IActionResult> GetMyRank(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var rank = await _leaderboardService.GetUserRankAsync(userId, cancellationToken);
        return Ok(new { UserId = userId, Rank = rank });
    }
}