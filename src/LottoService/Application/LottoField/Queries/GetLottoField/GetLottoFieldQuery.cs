using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.LottoField.Models;
using LottoService.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LottoService.Application.LottoField.Queries.GetLottoField
{
    public class GetLottoFieldQuery : IRequest<LottoFieldDto>
    {
    }

    public class GetLottoFieldQueryHandler : IRequestHandler<GetLottoFieldQuery, LottoFieldDto>
    {
        private readonly IRandomNumberService _randomNumberService;
        private readonly IRedisContext _redisContext;
        private readonly ILogger<GetLottoFieldQueryHandler> _logger;

        public GetLottoFieldQueryHandler(IRandomNumberService randomNumberService, IRedisContext redisContext, ILogger<GetLottoFieldQueryHandler> logger)
        {
            _randomNumberService = randomNumberService;
            _redisContext = redisContext;
            _logger = logger;
        }

        public async Task<LottoFieldDto> Handle(GetLottoFieldQuery request, CancellationToken cancellationToken)
        {
            // get data
            _logger.LogInformation("Getting lotto field numbers");
            int lottoMin = 1, lottoMax = 49, lottoCount = 6;
            Activity.Current?.SetTag("lotto.number.min", lottoMin).SetTag("lotto.number.max", lottoMax)
                .SetTag("lotto.number.count", lottoCount);
            _logger.LogTrace("Asking for {count} numbers between {min} and {max}", lottoCount, lottoMin, lottoMax);
            var lottoNumbers = await GetUniqueNumbers(lottoMin, lottoMax, lottoCount);
            _logger.LogInformation("Got lotto field data {@lottoFieldData}", lottoNumbers);

            _logger.LogInformation("Getting super number");
            int superMin = 1, superMax = 9;
            Activity.Current?.SetTag("superNumber.min", superMin).SetTag("superNumber.max", superMax);
            _logger.LogTrace("Asking for a number between {min} and {max}", superMin, superMax);
            var superNumber = await _randomNumberService.Generate(superMin, superMax);
            _logger.LogInformation("Got super number {superNumber}", superNumber);

            // create all data
            _logger.LogInformation("Validating values and create domain objects");
            var lottoField = new Domain.Entities.LottoField();
            lottoField.SetNumbers(lottoNumbers);
            _logger.LogInformation("Lotto numbers are valid");
            lottoField.SetSuperNumber(superNumber);
            _logger.LogInformation("Super number is valid");

            var lottoDto = new LottoFieldDto()
            {
                Numbers = lottoField.Numbers.Select(n => n.Number).ToList(),
                SuperNumber = lottoField.SuperNumber.Number
            };

            UpdateRedisData("lottoNumbers", lottoDto);

            return lottoDto;
        }

        // TODO: needs to go to antoher place. This is a first draft
        private void UpdateRedisData(string key, LottoFieldDto lottoFieldData)
        {
            var lottoData = _redisContext.GetAsArray<LottoFieldDto>(key);
            if (lottoData != null)
            {
                // take 10 and prepend another one, so 11 will be stored
                // in the retrieve method, skip 1 to just 10 will be returned by history query
                var newLottoData = lottoData.Take(10).Prepend(lottoFieldData);
                _redisContext.SetAsSeparatedItems(key, newLottoData);
            }
            else
            {
                _redisContext.Set(key, lottoFieldData);
            }
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
}
