using Microsoft.Extensions.Options;

using RandomNumberService.Application.Exceptions;
using RandomNumberService.Application.Interfaces;
using RandomNumberService.Config;

namespace RandomNumberService.Application.Services;

public class RandomNumberService : IRandomNumberService
{
    private readonly ILogger<RandomNumberService> _logger;
    private readonly AppConfig _appConfig;

    public RandomNumberService(ILogger<RandomNumberService> logger, IOptions<AppConfig> appConfig)
    {
        _logger = logger;
        _appConfig = appConfig.Value;
    }

    public int Generate(int min, int max)
    {
        _logger.LogInformation("Generating random number between {min} and {max}", min, max);

        if (min == max || min > max)
        {
            var ex = new ValueRangeException(min, max);
            _logger.LogError(ex, ex.Message);
            throw ex;
        }
        _logger.LogTrace("Min ({min}) and max ({max}) value are in valid ranges", min, max);

        var rng = new Random();
        var number = rng.Next(min, max);
        _logger.LogInformation("Generated number {number}", number);

        if (_appConfig.ThrowOnModulo > 0 &&
            number % _appConfig.ThrowOnModulo == 0)
        {
            throw new ModuloZeroException(number);
        }

        return number;
    }
}
