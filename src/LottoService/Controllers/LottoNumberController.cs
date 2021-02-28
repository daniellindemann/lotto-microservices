using System.Collections.Generic;
using System.Threading.Tasks;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.LottoField.Queries.GetLottoField;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LottoService.Controllers
{
    public class LottoNumberController : ApiControllerBase
    {
        private readonly ILogger<LottoNumberController> _logger;
        private readonly IRandomNumberService _randomNumberService;

        public LottoNumberController(ILogger<LottoNumberController> logger, IRandomNumberService randomNumberService)
        {
            _logger = logger;
            _randomNumberService = randomNumberService;
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
    }
}
