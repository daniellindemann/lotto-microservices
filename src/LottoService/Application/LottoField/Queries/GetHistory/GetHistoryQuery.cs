using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LottoService.Application.LottoField.Models;
using LottoService.Application.LottoField.Queries.GetLottoField;
using LottoService.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LottoService.Application.LottoField.Queries.GetHistory
{
    public class GetHistoryQuery : IRequest<IEnumerable<LottoFieldDto>>
    {
    }

    public class GetHistoryQueryHandler : IRequestHandler<GetHistoryQuery, IEnumerable<LottoFieldDto>>
    {
        private readonly IRedisContext _redisContext;
        private readonly ILogger<GetHistoryQueryHandler> _logger;

        public GetHistoryQueryHandler(IRedisContext redisContext, ILogger<GetHistoryQueryHandler> logger)
        {
            _redisContext = redisContext;
            _logger = logger;
        }

        public Task<IEnumerable<LottoFieldDto>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
        {
            var numToRetrieve = 10;
            var redisKey = "lottoNumbers";

            _logger.LogInformation("Retrieve last {numbersToRetrive} lotto numbers", numToRetrieve);
            var lottoNumbers = _redisContext.GetAsArray<LottoFieldDto>(redisKey);
            if(lottoNumbers != null)
                lottoNumbers = lottoNumbers.Skip(1).Take(numToRetrieve);
            _logger.LogInformation("Got data from redis: {@lottoFieldData}", lottoNumbers);

            return Task.FromResult(lottoNumbers);
        }
    }
}
