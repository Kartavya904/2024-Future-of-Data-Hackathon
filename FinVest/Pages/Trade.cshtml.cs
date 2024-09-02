using Microsoft.AspNetCore.Mvc.RazorPages;
using StockWave.Services;
using StockWave.Models;
using System.Threading.Tasks;

public class TradeModel : PageModel
{
    private readonly AlphaVantageService _alphaVantageService;

    public AlphaVantageResponse StockData { get; private set; }
    public double PortfolioValue { get; private set; } = 100000; // Starting portfolio value
    public int SharesOwned { get; private set; } = 0;

    public TradeModel(AlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    public async Task OnGetAsync(string symbol, string interval, string function)
    {
        if (!string.IsNullOrEmpty(symbol) && !string.IsNullOrEmpty(function) && !string.IsNullOrEmpty(interval))
        {
            StockData = await _alphaVantageService.GetStockData(symbol, interval, function);

            // For debugging purposes
            System.Diagnostics.Debug.WriteLine(StockData.TimeSeriesDaily.Count);
            foreach (var date in StockData.TimeSeriesDaily.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"{date}: {StockData.TimeSeriesDaily[date].Close}");
            }
        }
    }

    public void OnPostBuy(int shares)
    {
        // Implement the logic to buy shares and update PortfolioValue and SharesOwned
    }

    public void OnPostSell(int shares)
    {
        // Implement the logic to sell shares and update PortfolioValue and SharesOwned
    }
}
