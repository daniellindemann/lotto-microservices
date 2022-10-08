using LottoService.Application.Interfaces;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

using System.Text.Json;

namespace LottoService.Infrastructure.Services;

public class RedisCacheService<T> : AbstractRedisCacheService, ICacheService<T> where T : class, new()
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisConfig _redisConfig;
    private readonly ILogger<RedisCacheService<T>> _logger;

    public RedisCacheService(IDistributedCache distributedCache,
        IOptions<RedisConfig> redisConfig,
        ILogger<RedisCacheService<T>> logger) : base(distributedCache, redisConfig)
    {
        _distributedCache = distributedCache;
        _redisConfig = redisConfig.Value;
        _logger = logger;
    }

    public async Task<T?> GetAsync(string key)
    {
        var json = await GetStringAsync(key);
        if (json == null)
            return null;

        var obj = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        return obj;
    }

    public async Task SetAsync(string key, T? value)
    {
        if (value == null)
            return;

        var json = JsonSerializer.Serialize<T>(value);
        await SetStringAsync(key, json);
    }
}
