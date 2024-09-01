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

    public async Task OnGetAsync()
    {
        StockData = await _alphaVantageService.GetTimeSeriesDaily("AAPL"); // Example for Apple stock
    }

    public void OnPostBuy(int shares)
    {
        // Logic to handle buying shares
    }

    public void OnPostSell(int shares)
    {
        // Logic to handle selling shares
    }
}
