namespace GameLeaderboard.Application.DTOs;

public record AuthRequest(string DeviceId, string Username);

public record AuthResponse(string Token, Guid UserId, string Username);