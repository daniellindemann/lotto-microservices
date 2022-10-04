using Dapr.Client;

using LottoService.Application.Interfaces;
using LottoService.Config;
using LottoService.Domain.Entities;

using Microsoft.Extensions.Options;

namespace LottoService.Application.Services;

public class DaprCachedLottoNumberService : LottoNumberService
{
    private static readonly string CacheKey = "lottoFieldsTop100";
    private readonly DaprClient _daprClient;
    private readonly RedisConfig _redisConfig;

    public DaprCachedLottoNumberService(IRandomNumberService randomNumberService,
        DaprClient daprClient,
        IOptions<RedisConfig> redisConfig,
        IOptions<AppConfig> appConfig,
        ILogger<DaprCachedLottoNumberService> logger) : base(randomNumberService, appConfig, logger)
    {
        _daprClient = daprClient;
        _redisConfig = redisConfig.Value;
    }

    public override async Task<LottoField> Draw()
    {
        var lottoField = await base.Draw();

        await UpdateCache(lottoField);

        return lottoField;
    }

    public override async Task<List<LottoField>?> GetHistory()
    {
        var cachedLottoFields = await _daprClient.GetStateAsync<List<LottoField>>(_redisConfig.DaprStoreId, CacheKey);
        return cachedLottoFields;
    }

    private async Task UpdateCache(LottoField lottoField)
    {
        var cachedLottoFields = await GetHistory() ?? new List<LottoField>();
        cachedLottoFields?.Insert(0, lottoField);

        await _daprClient.SaveStateAsync(_redisConfig.DaprStoreId, CacheKey, cachedLottoFields);
    }
}
