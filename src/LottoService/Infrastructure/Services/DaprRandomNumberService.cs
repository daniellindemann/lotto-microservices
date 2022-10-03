using Dapr.Client;

using LottoService.Application.Interfaces;
using LottoService.Models.Requests;

namespace LottoService.Infrastructure.Services;

public class DaprRandomNumberService : IRandomNumberService
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<DaprRandomNumberService> _logger;

    public DaprRandomNumberService(DaprClient daprClient,
        ILogger<DaprRandomNumberService> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task<int> Generate(int min, int max)
    {
        _logger.LogInformation("Asking random number service for a new number");

        _logger.LogTrace("Creating http request object");
        var request = _daprClient.CreateInvokeMethodRequest<RandomNumberRequest>(
            HttpMethod.Post,
            "blub",
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
