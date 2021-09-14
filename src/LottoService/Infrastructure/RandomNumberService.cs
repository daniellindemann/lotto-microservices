using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.Configuration;
using Microsoft.Extensions.Logging;

namespace LottoService.Infrastructure
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly RandomNumberServiceSettings _randomNumberServiceSettings;
        private readonly ILogger<RandomNumberService> _logger;
        private readonly HttpClient _httpclient;

        public RandomNumberService(IHttpClientFactory httpClientFactory, RandomNumberServiceSettings randomNumberServiceSettings, ILogger<RandomNumberService> logger)
        {
            _randomNumberServiceSettings = randomNumberServiceSettings;
            _logger = logger;
            _httpclient = httpClientFactory.CreateClient();
        }

        public async Task<int> Generate(int min, int max)
        {
            _logger.LogInformation("Asking random number service for a new number");
                _logger.LogTrace("Creating http request object");
                var request = new HttpRequestMessage()
                {
                    Headers =
                    {
                        Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                    },
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_randomNumberServiceSettings.Url.TrimEnd('/')}/api/RandomNumber"),
                    Content = new StringContent($"{{ \"min\": {min}, \"max\": {max} }}", Encoding.UTF8, "application/json")
                };
                _logger.LogTrace("Request {@request}", request);
                Activity.Current?.SetTag("randomNumberService.Endpoint", request.RequestUri);

                _logger.LogTrace("Doing request");
                var response = await _httpclient.SendAsync(request);
                _logger.LogTrace("Response is {responseStatusCode}", response.StatusCode);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException();

                _logger.LogTrace("Reading content");
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogTrace("Response read, content is '{content}'", responseContent);
                _logger.LogTrace("Converting response to int");
                var number = Convert.ToInt32(responseContent);
                _logger.LogInformation("Returning number {number}", number);
                return Convert.ToInt32(number);
        }
    }
}
