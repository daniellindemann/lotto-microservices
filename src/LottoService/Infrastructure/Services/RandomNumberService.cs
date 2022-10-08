using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using LottoService.Application.Interfaces;
using LottoService.Config;
using LottoService.Models.Requests;

using Microsoft.Extensions.Options;

namespace LottoService.Infrastructure.Services;

public class RandomNumberService : IRandomNumberService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RandomNumberServiceConfig _randomNumberServiceConfig;
    private readonly ILogger<RandomNumberService> _logger;

    public RandomNumberService(IHttpClientFactory httpClientFactory,
        IOptions<RandomNumberServiceConfig> randomNumberServiceConfig,
        ILogger<RandomNumberService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _randomNumberServiceConfig = randomNumberServiceConfig.Value;
        _logger = logger;
    }

    public async Task<int> GenerateAsync(int min, int max)
    {
        _logger.LogInformation("Asking random number service for a new number");

        _logger.LogTrace("Creating http request object");
        var request = new HttpRequestMessage()
        {
            Headers =
            {
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_randomNumberServiceConfig.Url?.TrimEnd('/')}/api/RandomNumber"),
            Content = new StringContent(JsonSerializer.Serialize(new RandomNumberRequest()
            {
                Min = min,
                Max = max
            }), Encoding.UTF8, "application/json")
        };
        _logger.LogTrace("Request {@request}", request);

        _logger.LogTrace("Doing request");
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(request);
        _logger.LogTrace("Response is {responseStatusCode}", response.StatusCode);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException();

        _logger.LogTrace("Reading content");
        var responseContent = await response.Content.ReadAsStringAsync();
        _logger.LogTrace("Response read, content is '{content}'", responseContent);

        _logger.LogTrace("Converting response to int");
        var number = Convert.ToInt32(responseContent);
        _logger.LogInformation("Retrieved number {number}", number);

        return number;
    }
}
