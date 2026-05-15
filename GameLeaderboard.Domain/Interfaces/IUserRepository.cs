using GameLeaderboard.Domain.Entities;

namespace GameLeaderboard.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<User?> GetByDeviceIdAsync(string deviveId, CancellationToken cancellationToken = default);

    public Task<bool> IsUsernameTakenAsync(string username, Guid excludedUserId,
        CancellationToken cancellationToken = default);

    public Task AddAsync(User user, CancellationToken cancellationToken = default);
}