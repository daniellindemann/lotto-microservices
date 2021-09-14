using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RandomNumberService.Application;
using RandomNumberService.Application.Common.Exceptions;
using RandomNumberService.Application.Common.Interfaces;

namespace RandomNumberService.Infrastructure
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly ILogger<RandomNumberService> _logger;

        public RandomNumberService(ILogger<RandomNumberService> logger)
        {
            _logger = logger;
        }

        public int Generate(int min, int max)
        {
            Activity.Current?.SetTag("randomNumber.min", min).SetTag("randomNumber.max", max);

            _logger.LogInformation("Generating random number between {min} and {max}", min, max);
            if (min == max ||
                min > max)
            {
                var ex = new ValueRangeException(min, max);
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            _logger.LogTrace("Min ({min}) and max ({max}) value are in valid ranges", min, max);

            var rng = new Random();
            var number = rng.Next(min, max);
            _logger.LogInformation("Generated number {number}", number);

            return number;
        }
    }
}
