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
            var data = await Mediator.Send(new GetLottoFieldQuery());
            return data;
        }
    }
}
