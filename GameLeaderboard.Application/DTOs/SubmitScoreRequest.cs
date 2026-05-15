namespace GameLeaderboard.Application.DTOs;

public record SubmitScoreRequest(int Points, int Level, TimeSpan PlayTime);

public record ScoreResponse(Guid Id, int Points, int Level, TimeSpan PlayTime, DateTime CreatedAt);