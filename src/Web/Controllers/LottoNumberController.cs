using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Web.Application.Interfaces;
using Web.Config;
using Web.Models.Responses;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LottoNumberController
{
    private readonly LottoServiceConfig _lottoServiceConfig;
    private readonly ILottoNumberService _lottoNumberService;
    private readonly ILogger<LottoNumberController> _logger;

    public LottoNumberController(IOptions<LottoServiceConfig> lottoServiceConfig,
        ILottoNumberService lottoNumberService,
        ILogger<LottoNumberController> logger)
    {
        _lottoServiceConfig = lottoServiceConfig.Value;
        _lottoNumberService = lottoNumberService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<LottoFieldResponse> GetLottoNumber()
    {
        var response = await _lottoNumberService.GetLottoNumber();
        return response;
    }
}
