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
    private readonly IHostEnvironment _environment;
    private readonly LottoServiceConfig _lottoServiceConfig;
    private readonly DaprConfig _daprConfig;

    public AppConfigController(DaprClient daprClient,
        IOptions<LottoServiceConfig> lottoServiceConfig,
        IOptions<DaprConfig> daprConfig,
        IHostEnvironment environment)
    {
        _daprClient = daprClient;
        _environment = environment;
        _lottoServiceConfig = lottoServiceConfig.Value;
        _daprConfig = daprConfig.Value;
    }

    [HttpGet("get")]
    public AppConfigResponse GetConfig()
    {
        var variables = Environment.GetEnvironmentVariables();

        var daprRequestUri = ReplaceHomeAddressWithLocalhost(_daprClient.CreateInvokeMethodRequest(_lottoServiceConfig.DaprAppId, string.Empty)?.RequestUri);
        return new AppConfigResponse()
        {
            LottoService = new LottoServiceConfigResponse()
            {
                Url = _daprConfig.Enabled ?
                    daprRequestUri.ToString().TrimEnd('/') :
                    _lottoServiceConfig?.Url?.TrimEnd('/')
            }
        };
    }

    private Uri ReplaceHomeAddressWithLocalhost(Uri? requestUri)
    {
        if (requestUri == null)
            return null!;

        if (_environment.IsDevelopment())
        {
            var uriBuilder = new UriBuilder(requestUri);
            uriBuilder.Host = "localhost";
            return uriBuilder.Uri;
        }

        return requestUri;
    }
}

