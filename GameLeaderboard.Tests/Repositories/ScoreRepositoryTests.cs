using System;
using System.Threading.Tasks;
using FluentAssertions;
using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Infrastructure.Persistence;
using GameLeaderboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameLeaderboard.Tests.Repositories;

public class ScoreRepositoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ScoreRepository _repository;

    public ScoreRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new ScoreRepository(_dbContext);
    }

    [Fact]
    public async Task GetUserRankAsync_ShouldReturnCorrectRank_BasedOnTotalPoints()
    {
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var user3Id = Guid.NewGuid();

        await _dbContext.Scores.AddRangeAsync(
            new ScoreRecord { UserId = user1Id, Points = 50 },
            new ScoreRecord { UserId = user2Id, Points = 100 },
            new ScoreRecord { UserId = user2Id, Points = 50 },
            new ScoreRecord { UserId = user3Id, Points = 300 }
        );
        await _dbContext.SaveChangesAsync();

        var rankUser1 = await _repository.GetUserRankAsync(user1Id);
        var rankUser2 = await _repository.GetUserRankAsync(user2Id);
        var rankUser3 = await _repository.GetUserRankAsync(user3Id);

        rankUser3.Should().Be(1);
        rankUser2.Should().Be(2);
        rankUser1.Should().Be(3);
    }

    [Fact]
    public async Task GetUserRankAsync_ShouldReturnZero_IfUserHasNoScores()
    {
        var newUserId = Guid.NewGuid();
        var rank = await _repository.GetUserRankAsync(newUserId);
        rank.Should().Be(0);
    }
}