using Dapr.Client;

using LottoService.Application.Interfaces;
using LottoService.Config;
using LottoService.Models.Requests;

using Microsoft.Extensions.Options;

namespace LottoService.Infrastructure.Services;

public class DaprRandomNumberService : IRandomNumberService
{
    private readonly DaprClient _daprClient;
    private readonly RandomNumberServiceConfig _randomNumberServiceConfig;
    private readonly ILogger<DaprRandomNumberService> _logger;

    public DaprRandomNumberService(DaprClient daprClient,
        IOptions<RandomNumberServiceConfig> randomNumberServiceConfig,
        ILogger<DaprRandomNumberService> logger)
    {
        _daprClient = daprClient;
        _randomNumberServiceConfig = randomNumberServiceConfig.Value;
        _logger = logger;
    }

    public async Task<int> GenerateAsync(int min, int max)
    {
        _logger.LogInformation("Asking random number service for a new number");

        _logger.LogTrace("Creating http request object");
        var request = _daprClient.CreateInvokeMethodRequest<RandomNumberRequest>(
            HttpMethod.Post,
            _randomNumberServiceConfig.DaprAppId,
            "api/RandomNumber",
            new RandomNumberRequest()
            {
                Min = min,
                Max = max
            });
        _logger.LogTrace("Request {@request}", request);

        _logger.LogTrace("Doing request");
        var number = await _daprClient.InvokeMethodAsync<int>(request);
        _logger.LogInformation("Retrieved number {number}", number);

        return number;
    }
}
