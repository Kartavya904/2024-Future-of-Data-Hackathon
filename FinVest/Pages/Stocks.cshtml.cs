using Microsoft.AspNetCore.Mvc.RazorPages;
using StockWave.Services;
using StockWave.Models;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly AlphaVantageService _alphaVantageService;

    public AlphaVantageResponse StockData { get; private set; }

    public IndexModel(AlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    public async Task OnGetAsync(string symbol, string interval, string function)
    {
        if (!string.IsNullOrEmpty(symbol) && !string.IsNullOrEmpty(function))
        {
            StockData = await _alphaVantageService.GetStockData(symbol, interval, function);

            // For debugging purposes
            System.Diagnostics.Debug.WriteLine(StockData.TimeSeriesDaily.Count);
        }
    }
}
