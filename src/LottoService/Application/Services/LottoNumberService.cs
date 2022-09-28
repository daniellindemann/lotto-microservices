using LottoService.Application.Interfaces;
using LottoService.Config;
using LottoService.Domain.Entities;

using Microsoft.Extensions.Options;

namespace LottoService.Application.Services;

public class LottoNumberService : ILottoNumberService
{
    private readonly IRandomNumberService _randomNumberService;
    private readonly ILogger<LottoNumberService> _logger;
    private readonly AppConfig _appConfig;

    public LottoNumberService(IRandomNumberService randomNumberService,
        IOptions<AppConfig> appConfig,
        ILogger<LottoNumberService> logger)
    {
        _randomNumberService = randomNumberService;
        _logger = logger;
        _appConfig = appConfig.Value;
    }

    public async Task<LottoField> Draw()
    {
        _logger.LogInformation("Getting lotto field numbers");
        var lottoCount = _appConfig.NumberOfDraws;
        var lottoMin = _appConfig.LottoNumber.Min;
        var lottoMax = _appConfig.LottoNumber.Max;

        _logger.LogTrace("Asking for {count} numbers between {min} and {max}", lottoCount, lottoMin, lottoMax);
        var lottoNumbers = await GetUniqueNumbers(lottoMin, lottoMax, lottoCount);
        _logger.LogInformation("Got lotto field data {@lottoNumbers}", lottoNumbers);

        _logger.LogInformation("Getting super number");
        var superMin = _appConfig.SuperNumber.Min;
        var superMax = _appConfig.SuperNumber.Max;

        _logger.LogTrace("Asking for a number between {min} and {max}", superMin, superMax);
        var superNumber = await _randomNumberService.Generate(superMin, superMax);
        _logger.LogInformation("Got super number {superNumber}", superNumber);

        var lottoField = new LottoField();
        lottoField.SetNumbers(lottoNumbers);
        lottoField.SetSuperNumber(superNumber);

        return lottoField;
    }

    private async Task<IList<int>> GetUniqueNumbers(int min, int max, int count)
    {
        _logger.LogInformation("Getting {count} unique numbers between {min} and {max}", count, min, max);
        var uniqueNumbersCount = 0;
        var lottoNumbers = new List<int>();
        do
        {
            var numberOfRequests = count - uniqueNumbersCount;
            var lottoNumberTasks = new List<Task<int>>();
            for (int i = 0; i < numberOfRequests; i++)
            {
                lottoNumberTasks.Add(_randomNumberService.Generate(min, max));
            }

            var responseLottoNumbers = await Task.WhenAll(lottoNumberTasks);
            _logger.LogInformation("Got {count} numbers", responseLottoNumbers.Length);

            // filter
            lottoNumbers.AddRange(responseLottoNumbers);
            lottoNumbers = lottoNumbers.Distinct().ToList();
            _logger.LogInformation("{distinctCount} of {count} numbers are unique", lottoNumbers.Count, responseLottoNumbers.Length);

            uniqueNumbersCount = lottoNumbers.Count;

            if (uniqueNumbersCount < count)
            {
                _logger.LogInformation("Doing {count} more requests to get new numbers", count - uniqueNumbersCount);
            }
        } while (uniqueNumbersCount < count);

        return lottoNumbers;
    }
}
