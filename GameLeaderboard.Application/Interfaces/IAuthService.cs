using GameLeaderboard.Application.DTOs;
using GameLeaderboard.Domain.Interfaces;

namespace GameLeaderboard.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResponse> LoginOrRegisterAsync(AuthRequest request, CancellationToken cancellationToken = default);
}

public interface ILeaderboardService
{
    public Task SubmitScoreAsync(Guid userId, SubmitScoreRequest request,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<ScoreResponse>> GetUserRecentScoresAsync(Guid userId,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<UserScoreDto>> GetTopPlayersAsync(int top = 10, int offset = 0,
        CancellationToken cancellationToken = default);

    public Task<int> GetUserRankAsync(Guid userId, CancellationToken cancellationToken = default);
}