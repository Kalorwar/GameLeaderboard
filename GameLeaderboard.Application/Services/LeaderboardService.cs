using GameLeaderboard.Application.DTOs;
using GameLeaderboard.Application.Interfaces;
using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Domain.Exceptions;
using GameLeaderboard.Domain.Interfaces;

namespace GameLeaderboard.Application.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly ICacheService _cacheService;
    private readonly IScoreRepository _scoreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public LeaderboardService(IScoreRepository scoreRepository, IUserRepository userRepository, IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _scoreRepository = scoreRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task SubmitScoreAsync(Guid userId, SubmitScoreRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        var score = new ScoreRecord
        {
            UserId = userId,
            Points = request.Points,
            Level = request.Level,
            PlayTime = request.PlayTime
        };

        await _scoreRepository.AddAsync(score, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveByPrefixAsync("leaderboard_top_10_offset_0", cancellationToken);
    }

    public async Task<IEnumerable<ScoreResponse>> GetUserRecentScoresAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var scores = await _scoreRepository.GetRecentUserScoresAsync(userId, 10, cancellationToken);
        return scores.Select(s => new ScoreResponse(s.Id, s.Points, s.Level, s.PlayTime, s.CreatedAt));
    }

    public async Task<IEnumerable<UserScoreDto>> GetTopPlayersAsync(int top = 10, int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"leaderboard_top_{top}_offset_{offset}";

        var cachedLeaderboard = await _cacheService.GetAsync<IEnumerable<UserScoreDto>>(cacheKey, cancellationToken);
        if (cachedLeaderboard != null)
        {
            return cachedLeaderboard;
        }

        var dbLeaderboard = await _scoreRepository.GetTopPlayersAsync(top, offset, cancellationToken);

        await _cacheService.SetAsync(cacheKey, dbLeaderboard, TimeSpan.FromMinutes(2), cancellationToken);

        return dbLeaderboard;
    }

    public async Task<int> GetUserRankAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _scoreRepository.GetUserRankAsync(userId, cancellationToken);
    }
}