namespace GameLeaderboard.Domain.Exceptions;

public class ValidationException(string message) : DomainException(message);