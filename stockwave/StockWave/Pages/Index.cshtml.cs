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

    public async Task OnGetAsync()
    {
        StockData = await _alphaVantageService.GetTimeSeriesDaily("AAPL"); // Example for Apple stock
    }
}
