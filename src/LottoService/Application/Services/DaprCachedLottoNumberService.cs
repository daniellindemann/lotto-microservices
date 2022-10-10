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
    private readonly DaprConfig _daprConfig;

    public DaprCachedLottoNumberService(DaprClient daprClient,
        IHttpClientFactory httpClientFactory,
        IOptions<DaprConfig> daprConfig,
        IRandomNumberService randomNumberService,
        IOptions<AppConfig> appConfig,
        ILogger<DaprCachedLottoNumberService> logger) : base(randomNumberService, appConfig, logger)
    {
        _daprClient = daprClient;
        _httpClientFactory = httpClientFactory;
        _daprConfig = daprConfig.Value;
    }

    public override async Task<LottoField> DrawAsync()
    {
        var lottoField = await base.DrawAsync();

        await UpdateCache(lottoField);

        return lottoField;
    }

    public override async Task<List<LottoField>?> GetHistoryAsync()
    {
        if (UseHttp)
        {
            return await GetHistoryHttpAsync();
        }
        else
        {
            return await GetHistoryDaprClientAsync();
        }
    }

    private async Task UpdateCache(LottoField lottoField)
    {
        var cachedLottoFields = await GetHistoryAsync() ?? new List<LottoField>();
        cachedLottoFields?.Insert(0, lottoField);

        if (UseHttp)
        {
            await UpdateCacheHttpAsync(cachedLottoFields);
        }
        else
        {
            await UpdateCacheDaprClientAsync(cachedLottoFields);
        }
    }

    private async Task<List<LottoField>?> GetHistoryDaprClientAsync()
    {
        var cachedLottoFields = await _daprClient.GetStateAsync<List<LottoField>>(_daprConfig.DaprStoreId, CacheKey);
        return cachedLottoFields;
    }

    private async Task UpdateCacheDaprClientAsync(List<LottoField>? lottoFields)
    {
        await _daprClient.SaveStateAsync(_daprConfig.DaprStoreId, CacheKey, lottoFields);
    }

    private async Task<List<LottoField>?> GetHistoryHttpAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var url = $"http://localhost:3500/v1.0/state/{_daprConfig.DaprStoreId}/{CacheKey}";    // yep, is hardcoded for demo purpose; will not work with tye and dapr config
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

    private async Task UpdateCacheHttpAsync(List<LottoField>? lottoFields)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var url = $"http://localhost:3500/v1.0/state/{_daprConfig.DaprStoreId}";    // yep, url is hardcoded for demo purpose; will not work with tye and dapr config
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
}
