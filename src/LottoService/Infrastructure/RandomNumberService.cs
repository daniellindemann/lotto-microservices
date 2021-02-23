using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.Configuration;

namespace LottoService.Infrastructure
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly RandomNumberServiceSettings _randomNumberServiceSettings;
        private readonly HttpClient _httpclient;

        public RandomNumberService(IHttpClientFactory httpClientFactory, RandomNumberServiceSettings randomNumberServiceSettings)
        {
            _randomNumberServiceSettings = randomNumberServiceSettings;
            _httpclient = httpClientFactory.CreateClient();
        }

        public async Task<int> Generate(int min, int max)
        {
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

            var response = await _httpclient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException();

            var responseContent = await response.Content.ReadAsStringAsync();
            return Convert.ToInt32(responseContent);
        }
    }
}
