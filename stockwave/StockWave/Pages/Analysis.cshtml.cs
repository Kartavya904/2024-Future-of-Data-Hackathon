using Microsoft.AspNetCore.Mvc.RazorPages;
using StockWave.Services;
using StockWave.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnalysisModel : PageModel
{
    private readonly AlphaVantageService _alphaVantageService;

    public AlphaVantageResponse StockData { get; private set; }
    public List<string> AnalysisResults { get; private set; } = new List<string>();

    public AnalysisModel(AlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    public async Task OnGetAsync(string symbol)
    {
        StockData = await _alphaVantageService.GetTimeSeriesDaily(symbol);

        // Example of adding some analysis logic
        foreach (var date in StockData.TimeSeriesDaily.Keys)
        {
            var closingPrice = StockData.TimeSeriesDaily[date].Close;
            AnalysisResults.Add($"On {date}, the closing price was {closingPrice}.");
        }
    }
}
