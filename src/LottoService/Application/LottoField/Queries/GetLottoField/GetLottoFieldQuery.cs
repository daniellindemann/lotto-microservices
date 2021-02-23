using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LottoService.Application.Common.Interfaces;
using MediatR;

namespace LottoService.Application.LottoField.Queries.GetLottoField
{
    public class GetLottoFieldQuery : IRequest<LottoFieldDto>
    {
    }

    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetLottoFieldQuery, LottoFieldDto>
    {
        private readonly IRandomNumberService _randomNumberService;

        public GetTodoItemsWithPaginationQueryHandler(IRandomNumberService randomNumberService)
        {
            _randomNumberService = randomNumberService;
        }

        public async Task<LottoFieldDto> Handle(GetLottoFieldQuery request, CancellationToken cancellationToken)
        {
            // get data
            var lottoNumbers = await GetUniqueNumbers(1, 49, 6);
            var superNumber = await _randomNumberService.Generate(1, 9);

            // create all data
            var lottoField = new Domain.Entities.LottoField();
            lottoField.SetNumbers(lottoNumbers);
            lottoField.SetSuperNumber(superNumber);

            return new LottoFieldDto()
            {
                Numbers = lottoField.Numbers.Select(n => n.Number).ToList(),
                SuperNumber = lottoField.SuperNumber.Number
            };
        }

        private async Task<IList<int>> GetUniqueNumbers(int min, int max, int count)
        {
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

                // filter
                lottoNumbers.AddRange(responseLottoNumbers);
                lottoNumbers = lottoNumbers.Distinct().ToList();

                uniqueNumbersCount = lottoNumbers.Count;
            } while (uniqueNumbersCount < count);

            return lottoNumbers;
        }
    }
}
