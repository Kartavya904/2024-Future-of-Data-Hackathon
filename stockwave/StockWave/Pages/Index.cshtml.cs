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
        StockData = await _alphaVantageService.GetTimeSeriesDaily("CSSEQ"); // Example for Apple stock

        // For debugging purposes
        System.Diagnostics.Debug.WriteLine(StockData.TimeSeriesDaily.Count); // Print the number of data points
        foreach(var date in StockData.TimeSeriesDaily.Keys)
        {
            System.Diagnostics.Debug.WriteLine($"{date}: {StockData.TimeSeriesDaily[date].Close}");
        }
    }

}
