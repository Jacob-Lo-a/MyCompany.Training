using Training.Core;
using Training.Core.interfaces;
namespace Training.API.Services
{
    public class TaipeiWeatherService : ITaipeiWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TaipeiWeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetWeatherAsync()
        {
            var client = _httpClientFactory.CreateClient("CwaClient");

            var response = await client.GetAsync(
                "api/v1/rest/datastore/F-D0047-061?Authorization=CWB-E77DCF53-A8BF-4D32-994E-8643293F891F"
            );
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("呼叫氣象 API 失敗");
            } 

            return await response.Content.ReadAsStringAsync();
        }
    }
} 
