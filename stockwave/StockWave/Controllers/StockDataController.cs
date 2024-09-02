using Microsoft.AspNetCore.Mvc;
using StockWave.Services;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class StockDataController : ControllerBase
{
    private readonly AlphaVantageService _alphaVantageService;

    public StockDataController(AlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStockData(string symbol, string interval, string function)
    {
        var stockData = await _alphaVantageService.GetStockData(symbol, interval, function);
        return Ok(stockData);
    }
}
