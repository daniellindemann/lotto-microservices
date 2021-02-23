using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RandomNumberService.Application;
using RandomNumberService.Application.Common.Interfaces;

namespace RandomNumberService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RandomNumberController : ControllerBase
    {
        private readonly IRandomNumberService _randomNumberService;
        private readonly ILogger<RandomNumberController> _logger;

        public RandomNumberController(IRandomNumberService randomNumberService, ILogger<RandomNumberController> logger)
        {
            _randomNumberService = randomNumberService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetRandomNumber(int min, int max)
        {
            var number = _randomNumberService.Generate(min, max);
            return Ok(number);
        }

        [HttpPost]
        public IActionResult GetRandomNumber([FromBody] GetRandomNumberRequest requestData)
        {
            var number = _randomNumberService.Generate(requestData.Min, requestData.Max);
            return Ok(number);
        }

        public class GetRandomNumberRequest
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }
    }
}
