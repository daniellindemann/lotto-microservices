using Dapr.Client;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Web.Config;
using Web.Models.Responses;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppConfigController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly LottoServiceConfig _lottoServiceConfig;
    private readonly DaprConfig _daprConfig;

    public AppConfigController(DaprClient daprClient,
        IOptions<LottoServiceConfig> lottoServiceConfig,
        IOptions<DaprConfig> daprConfig)
    {
        _daprClient = daprClient;
        _lottoServiceConfig = lottoServiceConfig.Value;
        _daprConfig = daprConfig.Value;
    }

    [HttpGet("get")]
    public AppConfigResponse GetConfig()
    {
        return new AppConfigResponse()
        {
            LottoService = new LottoServiceConfigResponse()
            {
                Url = _daprConfig.Enabled ?
                    _daprClient.CreateInvokeMethodRequest(_lottoServiceConfig.DaprAppId, string.Empty)?.RequestUri?.ToString().TrimEnd('/') :
                    _lottoServiceConfig?.Url?.TrimEnd('/')
            }
        };
    }
}

