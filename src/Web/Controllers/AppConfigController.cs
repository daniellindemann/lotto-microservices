using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Web.Config;
using Web.Models.Responses;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppConfigController : ControllerBase
{
    private readonly LottoServiceConfig _lottoServiceConfig;

    public AppConfigController(IOptions<LottoServiceConfig> lottoServiceConfig)
    {
        _lottoServiceConfig = lottoServiceConfig.Value;
    }

    [HttpGet("get")]
    public AppConfigResponse GetConfig()
    {
        return new AppConfigResponse()
        {
            LottoService = new LottoServiceConfigResponse()
            {
                Url = _lottoServiceConfig.Url
            }
        };
    }
}

