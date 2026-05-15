namespace GameLeaderboard.Domain.Exceptions;

public class NotFoundException(string entityName, object key)
    : DomainException($"Entity '{entityName}' with key '{key}' was not found.");