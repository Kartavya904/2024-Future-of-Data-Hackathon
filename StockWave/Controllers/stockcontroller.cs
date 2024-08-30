using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockWave.Controllers
{
    public class StockController : Controller
    {
        private static readonly string API_KEY = "7472XEMETXL0GFZF"; // Replace with your actual API key
        private static readonly string BASE_URL = "https://www.alphavantage.co/query";

        public async Task<IActionResult> Index(string symbol = "ES1!")
        {
            var stockData = await GetStockData(symbol);
            return View(stockData);
        }

        private async Task<Dictionary<string, Dictionary<string, string>>> GetStockData(string symbol)
        {
            string queryUrl = $"{BASE_URL}?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&apikey={API_KEY}";
            Uri queryUri = new Uri(queryUrl);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(queryUri);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var stockData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonResponse);

                    var result = new Dictionary<string, Dictionary<string, string>>(); // Correct type for view

                    if (stockData.TryGetValue("Time Series (5min)", out JsonElement timeSeriesData))
                    {
                        foreach (var timeEntry in timeSeriesData.EnumerateObject())
                        {
                            var innerDict = new Dictionary<string, string>();
                            foreach (var priceData in timeEntry.Value.EnumerateObject())
                            {
                                innerDict[priceData.Name] = priceData.Value.GetString();
                            }
                            result[timeEntry.Name] = innerDict;
                        }
                    }

                    return result;
                }
                else
                {
                    // Handle error response
                    return null;
                }
            }
        }
    }
}
