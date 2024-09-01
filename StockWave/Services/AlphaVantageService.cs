using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockWave.Models;

namespace StockWave.Services
{
    public class AlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "1FQKLFV7X0SPA10I"; // Replace with your API key

        public AlphaVantageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AlphaVantageResponse> GetTimeSeriesDaily(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apiKey}";
            var response = await _httpClient.GetFromJsonAsync<AlphaVantageResponse>(url);
            return response;
        }
    }
}
