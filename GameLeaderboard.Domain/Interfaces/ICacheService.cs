namespace GameLeaderboard.Domain.Interfaces;

public interface ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    public Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default);

    public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}