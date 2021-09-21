using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Web.Config;

namespace Web.Data
{
    public class LottoService
    {
        private readonly LottoServiceConfig _lottoServiceConfig;
        private readonly HttpClient _httpClient;

        public LottoService(IHttpClientFactory httpClientFactory, LottoServiceConfig lottoServiceConfig)
        {
            _httpClient = httpClientFactory.CreateClient();
            _lottoServiceConfig = lottoServiceConfig;
        }

        public async Task<LottoNumbers> GetNumbers()
        {
            var response = await _httpClient.GetAsync($"{_lottoServiceConfig.Url.TrimEnd('/')}/api/LottoNumber");
            var data = JsonConvert.DeserializeObject<LottoNumbers>(await response.Content.ReadAsStringAsync());

            return data;
        }

        public async Task<IEnumerable<LottoNumbers>> GetHistory()
        {
            var response = await _httpClient.GetAsync($"{_lottoServiceConfig.Url.TrimEnd('/')}/api/LottoNumber/history");
            var data = JsonConvert.DeserializeObject<IEnumerable<LottoNumbers>>(
                await response.Content.ReadAsStringAsync());

            return data;
        }

        public class LottoNumbers
        {
            public List<int> Numbers { get; set; }
            public int SuperNumber { get; set; }
        }
    }
}
