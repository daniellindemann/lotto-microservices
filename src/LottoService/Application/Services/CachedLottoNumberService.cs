using LottoService.Application.Interfaces;
using LottoService.Config;
using LottoService.Domain.Entities;

using Microsoft.Extensions.Options;

namespace LottoService.Application.Services;

public class CachedLottoNumberService : LottoNumberService
{
    private static readonly string CacheKey = "lottoFieldsTop100";

    private readonly ICacheService<List<LottoField>> _cacheService;

    public CachedLottoNumberService(IRandomNumberService randomNumberService,
        ICacheService<List<LottoField>> cacheService,
        IOptions<AppConfig> appConfig,
        ILogger<CachedLottoNumberService> logger) : base(randomNumberService, appConfig, logger)
    {
        _cacheService = cacheService;
    }

    public override async Task<LottoField> DrawAsync()
    {
        var lottoField = await base.DrawAsync();

        await UpdateCache(lottoField);

        return lottoField;
    }

    public override async Task<List<LottoField>?> GetHistoryAsync()
    {
        var cachedLottoFields = await _cacheService.GetAsync(CacheKey) ?? null;
        return cachedLottoFields;
    }

    private async Task UpdateCache(LottoField lottoField)
    {
        var cachedLottoFields = await GetHistoryAsync() ?? new List<LottoField>();
        cachedLottoFields?.Insert(0, lottoField);

        await _cacheService.SetAsync(CacheKey, cachedLottoFields?.Take(100).ToList());
    }
}
