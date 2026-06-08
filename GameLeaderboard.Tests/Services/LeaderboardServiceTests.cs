using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameLeaderboard.Application.DTOs;
using GameLeaderboard.Application.Services;
using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Domain.Exceptions;
using GameLeaderboard.Domain.Interfaces;
using Moq;
using Xunit;

namespace GameLeaderboard.Tests.Services;

public class LeaderboardServiceTests
{
    private readonly Mock<ICacheService> _cacheMock;
    private readonly Mock<IScoreRepository> _scoreRepoMock;
    private readonly LeaderboardService _service;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IUserRepository> _userRepoMock;

    public LeaderboardServiceTests()
    {
        _scoreRepoMock = new Mock<IScoreRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _cacheMock = new Mock<ICacheService>();

        _service = new LeaderboardService(_scoreRepoMock.Object, _userRepoMock.Object, _uowMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task SubmitScoreAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var fakeUserId = Guid.NewGuid();
        var request = new SubmitScoreRequest(100, 1, TimeSpan.FromMinutes(2));

        _userRepoMock.Setup(repo => repo.GetByIdAsync(fakeUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);


        var act = async () => await _service.SubmitScoreAsync(fakeUserId, request);


        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{fakeUserId}*not found*");

        _scoreRepoMock.Verify(repo => repo.AddAsync(It.IsAny<ScoreRecord>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _cacheMock.Verify(cache => cache.RemoveByPrefixAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}