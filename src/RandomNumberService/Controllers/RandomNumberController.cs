using Microsoft.AspNetCore.Mvc;

using RandomNumberService.Application.Interfaces;
using RandomNumberService.Models.Requests;

namespace RandomNumberService.Controllers;

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
        _logger.LogInformation("Received GET request with {min} and {max} value", min, max);
        var number = _randomNumberService.Generate(min, max);
        _logger.LogInformation("Returning http response with number {number}", number);
        return Ok(number);
    }

    [HttpPost]
    public IActionResult GetRandomNumber([FromBody] GetRandomNumberRequest requestData)
    {
        _logger.LogInformation("Received POST request with {min} and {max} value", requestData.Min, requestData.Max);
        var number = _randomNumberService.Generate(requestData.Min, requestData.Max);
        _logger.LogInformation("Returning http response with number {number}", number);
        return Ok(number);
    }
}
