using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Domain.Interfaces;
using GameLeaderboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameLeaderboard.Infrastructure.Repositories;

public class ScoreRepository(ApplicationDbContext context) : IScoreRepository
{
    public async Task AddAsync(ScoreRecord score, CancellationToken cancellationToken = default)
    {
        await context.Scores.AddAsync(score, cancellationToken);
    }

    public async Task<IEnumerable<ScoreRecord>> GetRecentUserScoresAsync(Guid userId, int count = 10,
        CancellationToken cancellationToken = default)
    {
        return await context.Scores
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserScoreDto>> GetTopPlayersAsync(int top, int offset,
        CancellationToken cancellationToken = default)
    {
        var query = context.Scores.GroupBy(s => s.User!)
            .Select(g => new
            {
                User = g.Key,
                TotalPoints = g.Sum(x => x.Points)
            })
            .OrderByDescending(x => x.TotalPoints)
            .Skip(offset)
            .Take(top);

        var result = await query.ToListAsync(cancellationToken);

        return result.Select((r, index) => new UserScoreDto(
            r.User.Id,
            r.User.Username,
            r.TotalPoints,
            offset + index + 1
        ));
    }

    public async Task<int> GetUserRankAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userTotalPoints = await context.Scores
            .Where(s => s.UserId == userId)
            .SumAsync(s => s.Points, cancellationToken);

        if (userTotalPoints == 0)
        {
            return 0;
        }

        var higherScoringPlayersCount = await context.Scores
            .GroupBy(s => s.UserId)
            .Select(g => g.Sum(x => x.Points))
            .CountAsync(sum => sum > userTotalPoints, cancellationToken);

        return higherScoringPlayersCount + 1;
    }
}