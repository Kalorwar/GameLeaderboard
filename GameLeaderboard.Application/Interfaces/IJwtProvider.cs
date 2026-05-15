using GameLeaderboard.Domain.Entities;

namespace GameLeaderboard.Application.Interfaces;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}