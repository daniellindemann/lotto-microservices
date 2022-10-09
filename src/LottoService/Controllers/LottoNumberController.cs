using LottoService.Application.Interfaces;
using LottoService.Models.Responses;

using Microsoft.AspNetCore.Mvc;

namespace LottoService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LottoNumberController : ControllerBase
{
    private readonly ILottoNumberService _lottoNumberService;
    private readonly ILogger<LottoNumberController> _logger;

    public LottoNumberController(ILottoNumberService lottoNumberService,
        ILogger<LottoNumberController> logger)
    {
        _lottoNumberService = lottoNumberService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<LottoFieldResponse> GetAsync()
    {
        _logger.LogInformation("Retrieved get request");

        _logger.LogTrace("Calling lotto number service");
        var lottoField = await _lottoNumberService.DrawAsync();
        _logger.LogTrace("Got lotto numbers @{lottoNumbers}", lottoField);

        // create custom response object
        var response = new LottoFieldResponse()
        {
            Numbers = lottoField.Numbers,
            SuperNumber = lottoField.SuperNumber
        };

        return response;
    }

    [HttpGet("history")]
    public async Task<List<LottoFieldResponse>?> GetHistoryAsync()
    {
        _logger.LogInformation("Retrieved get request");

        _logger.LogTrace("Calling lotto number service to retrieve history");
        var lottoField = await _lottoNumberService.GetHistoryAsync();
        _logger.LogTrace("Got lotto numbers history @{lottoNumbers}", lottoField);

        // create custom response object
        var response = lottoField?.Select(lf => new LottoFieldResponse()
        {
            Numbers = lf.Numbers,
            SuperNumber = lf.SuperNumber
        }).ToList();

        return response;
    }
}
