using GameLeaderboard.Domain.Entities;

namespace GameLeaderboard.Domain.Interfaces;

public interface IScoreRepository
{
    public Task AddAsync(ScoreRecord score, CancellationToken cancellationToken = default);
   
    public Task<IEnumerable<ScoreRecord>> GetRecentUserScoresAsync(Guid userId, int count = 10, CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<UserScoreDto>> GetTopPlayersAsync(int top, int offset, CancellationToken cancellationToken = default);
    
    public Task<int> GetUserRankAsync(Guid userId, CancellationToken cancellationToken = default);
}

public record UserScoreDto(Guid UserId, string Username, int TotalPoints, int Rank = 0);