using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace LottoService.Infrastructure.Services;

public abstract class AbstractRedisCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisConfig _redisConfig;

    protected AbstractRedisCacheService(IDistributedCache distributedCache,
        IOptions<RedisConfig> redisConfig)
    {
        _distributedCache = distributedCache;
        _redisConfig = redisConfig.Value;
    }

    public async Task<string?> GetStringAsync(string key)
    {
        if (!_redisConfig.Enabled)
            return null;

        return await _distributedCache.GetStringAsync(key);
    }

    public async Task SetStringAsync(string key, string value)
    {
        if (!_redisConfig.Enabled)
            return;

        await _distributedCache.SetStringAsync(key, value);
    }
}
