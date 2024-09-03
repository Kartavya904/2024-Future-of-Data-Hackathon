using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StockWave.Models;

namespace StockWave.Services
{
    public class AlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "XRD89UZWKY7V5DCX"; // Replace with your API key

        public AlphaVantageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AlphaVantageResponse> GetStockData(string symbol, string interval, string function)
        {
            string url;

            if (function == "TIME_SERIES_INTRADAY")
            {
                // For intraday data, include the interval
                url = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&interval={interval}&apikey={_apiKey}";
            }
            else
            {
                // For daily, weekly, and monthly data, no interval is needed
                url = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&apikey={_apiKey}";
            }

            var response = await _httpClient.GetFromJsonAsync<AlphaVantageResponse>(url);
            return response;
        }
    }
}
