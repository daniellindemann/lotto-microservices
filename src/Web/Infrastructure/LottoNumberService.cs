using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.Options;

using Web.Application.Interfaces;
using Web.Config;
using Web.Models.Responses;

namespace Web.Infrastructure;

public class LottoNumberService : ILottoNumberService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly LottoServiceConfig _lottoServiceConfig;
    private readonly ILogger<LottoNumberService> _logger;

    public LottoNumberService(
        IHttpClientFactory httpClientFactory,
        IOptions<LottoServiceConfig> lottoServiceConfig,
        ILogger<LottoNumberService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _lottoServiceConfig = lottoServiceConfig.Value;
        _logger = logger;
    }
    
    public async Task<LottoFieldResponse> GetLottoNumber()
    {
        _logger.LogInformation("Build request object");
        var request = new HttpRequestMessage()
        {
            Headers = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } },
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_lottoServiceConfig.Url?.TrimEnd('/')}/api/LottoNumber")
        };
        
        _logger.LogInformation("Send http request");
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(request);
        
        _logger.LogInformation("Check http response");
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException();
        
        _logger.LogInformation("Build response object");
        var responseContent = await response.Content.ReadAsStringAsync();
        
        var lottoFieldResponse = JsonSerializer.Deserialize<LottoFieldResponse>(responseContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        return lottoFieldResponse!;
    }
}