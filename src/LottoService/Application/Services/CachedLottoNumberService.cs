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

    public override async Task<LottoField> Draw()
    {
        var lottoField = await base.Draw();

        await UpdateCache(lottoField);

        return lottoField;
    }

    public override async Task<List<LottoField>?> GetHistory()
    {
        var cachedLottoFields = await _cacheService.Get(CacheKey) ?? null;
        return cachedLottoFields;
    }

    private async Task UpdateCache(LottoField lottoField)
    {
        var cachedLottoFields = await GetHistory() ?? new List<LottoField>();
        cachedLottoFields?.Insert(0, lottoField);

        await _cacheService.Set(CacheKey, cachedLottoFields?.Take(100).ToList());
    }
}
