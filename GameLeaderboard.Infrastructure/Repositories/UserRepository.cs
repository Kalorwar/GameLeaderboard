using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Domain.Interfaces;
using GameLeaderboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameLeaderboard.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.DeviceId == deviceId, cancellationToken);
    }

    public async Task<bool> IsUsernameTakenAsync(string username, Guid excludeUserId,
        CancellationToken cancellationToken = default)
    {
        return await context.Users.AnyAsync(u => u.Username == username && u.Id != excludeUserId, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }
}