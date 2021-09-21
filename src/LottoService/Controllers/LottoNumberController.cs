using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.LottoField.Models;
using LottoService.Application.LottoField.Queries.GetHistory;
using LottoService.Application.LottoField.Queries.GetLottoField;
using LottoService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LottoService.Controllers
{
    public class LottoNumberController : ApiControllerBase
    {
        private readonly ILogger<LottoNumberController> _logger;

        public LottoNumberController(ILogger<LottoNumberController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<LottoFieldDto> Get()
        {
            _logger.LogInformation("Retrieved get request");

            _logger.LogTrace("Creating mediator request");
            var mediatorRequest = new GetLottoFieldQuery();
            _logger.LogTrace("Created {name} request", nameof(GetLottoFieldQuery));

            var data = await Mediator.Send(mediatorRequest);
            _logger.LogInformation("Lotto data is {@lottoData}", data);
            return data;
        }

        [HttpGet("history")]
        public async Task<IEnumerable<LottoFieldDto>> GetHistory()
        {
            _logger.LogInformation("Retrieved get history request");

            _logger.LogTrace("Creating mediator request");
            var mediatorRequest = new GetHistoryQuery();
            _logger.LogTrace("Created {name} request", nameof(GetLottoFieldQuery));

            var data = await Mediator.Send(mediatorRequest);
            _logger.LogInformation("History data is {@historyData}", data);
            return data;
        }
    }
}
