using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Dapr.Client;

using LottoService.Application.Interfaces;
using LottoService.Config;
using LottoService.Domain.Entities;
using LottoService.Models.Requests;

using Microsoft.Extensions.Options;

namespace LottoService.Application.Services;

public class DaprCachedLottoNumberService : LottoNumberService
{
    public static readonly bool UseHttp = false;
    private static readonly string CacheKey = "lottoFieldsTop100";

    private readonly DaprClient _daprClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RedisConfig _redisConfig;

    public DaprCachedLottoNumberService(DaprClient daprClient,
        IHttpClientFactory httpClientFactory,
        IOptions<RedisConfig> redisConfig,
        IRandomNumberService randomNumberService,
        IOptions<AppConfig> appConfig,
        ILogger<DaprCachedLottoNumberService> logger) : base(randomNumberService, appConfig, logger)
    {
        _daprClient = daprClient;
        _httpClientFactory = httpClientFactory;
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
        if (UseHttp)
        {
            return await GetHistoryHttp();
        }
        else
        {
            return await GetHistoryDaprClient();
        }
    }

    private async Task<List<LottoField>?> GetHistoryDaprClient()
    {
        var cachedLottoFields = await _daprClient.GetStateAsync<List<LottoField>>(_redisConfig.DaprStoreId, CacheKey);
        return cachedLottoFields;
    }

    private async Task<List<LottoField>?> GetHistoryHttp()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var url = $"http://localhost:3500/v1.0/state/statestore/{CacheKey}";    // yep, it's hardcoded for demo purpose
        var request = new HttpRequestMessage()
        {
            Headers =
            {
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        };

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new KeyNotFoundException($"No data on key {CacheKey}");

        var json = await response.Content.ReadAsStringAsync();

        var cachedLottoFields = json == string.Empty ? null : JsonSerializer.Deserialize<List<LottoField>>(json);
        return cachedLottoFields;
    }

    private async Task UpdateCache(LottoField lottoField)
    {
        var cachedLottoFields = await GetHistory() ?? new List<LottoField>();
        cachedLottoFields?.Insert(0, lottoField);

        if (UseHttp)
        {
            await UpdateCacheHttp(cachedLottoFields);
        }
        else
        {
            await UpdateCacheDaprClient(cachedLottoFields);
        }
    }

    private async Task UpdateCacheHttp(List<LottoField>? lottoFields)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var url = "http://localhost:3500/v1.0/state/statestore";
        var request = new HttpRequestMessage()
        {
            Headers =
            {
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new StringContent(JsonSerializer.Serialize(new[] {
                new StateStoreHttpData<List<LottoField>>()
                {
                    Key = CacheKey,
                    Value = lottoFields
                }
            }), Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new KeyNotFoundException($"No data on key {CacheKey}");
    }

    private async Task UpdateCacheDaprClient(List<LottoField>? lottoFields)
    {
        await _daprClient.SaveStateAsync(_redisConfig.DaprStoreId, CacheKey, lottoFields);
    }
}
