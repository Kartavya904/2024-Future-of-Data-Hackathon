using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class TradingController : Controller
{
    private static UserPortfolio _portfolio = new UserPortfolio();
    private readonly HttpClient _httpClient;

    public TradingController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Balance = _portfolio.Balance;
        ViewBag.Holdings = _portfolio.Holdings ?? new Dictionary<string, Stock>(); // Make sure Holdings is not null
        ViewBag.Portfolio = _portfolio; // Pass the portfolio for profit margin calculation
        return View(_portfolio);
    }

    [HttpPost]
    public async Task<IActionResult> BuyStock(string symbol, int shares)
    {
        string apiKey = "XRD89UZWKY7V5DCX";
        var stock = await GetStockPrice(symbol, apiKey);

        if (stock != null)
        {
            decimal cost = stock.Price * shares;
            if (cost <= _portfolio.Balance)
            {
                _portfolio.Balance -= cost;
                if (_portfolio.Holdings.ContainsKey(stock.Symbol))
                {
                    _portfolio.Holdings[stock.Symbol].Shares += shares;
                }
                else
                {
                    stock.PurchasePrice = stock.Price;
                    stock.Shares = shares;
                    _portfolio.Holdings[stock.Symbol] = stock;
                }

                TempData["Message"] = $"Purchased {shares} shares of {stock.Symbol} at {stock.Price:C} each. Remaining balance: {_portfolio.Balance:C}";
            }
            else
            {
                TempData["Message"] = "Insufficient balance.";
            }
        }
        else
        {
            // Handle the case where the stock was not found
            TempData["Message"] = "Could not buy stock. Please check the stock symbol and try again.";
        }

        ViewBag.Balance = _portfolio.Balance;
        ViewBag.Holdings = _portfolio.Holdings;
        return View("Index", _portfolio);
    }

    [HttpPost]
    public async Task<IActionResult> SellStock(string symbol, int shares)
    {
        string apiKey = "XRD89UZWKY7V5DCX";
        var currentStock = await GetStockPrice(symbol, apiKey);

        if (currentStock != null && _portfolio.Holdings.ContainsKey(symbol))
        {
            var stock = _portfolio.Holdings[symbol];
            if (shares <= stock.Shares)
            {
                decimal proceeds = currentStock.Price * shares;
                _portfolio.Balance += proceeds;
                stock.Shares -= shares;

                if (stock.Shares == 0)
                {
                    _portfolio.Holdings.Remove(symbol);
                }
                else
                {
                    _portfolio.Holdings[symbol] = stock; // Update the stock in holdings
                }

                TempData["Message"] = $"Sold {shares} shares of {stock.Symbol} at {currentStock.Price:C} each. Current balance: {_portfolio.Balance:C}";
            }
            else
            {
                TempData["Message"] = "Not enough shares to sell.";
            }
        }
        else
        {
            TempData["Message"] = "Stock not found or you don't own any shares of this stock.";
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> CheckStockPrice(string symbol)
    {
        string apiKey = "XRD89UZWKY7V5DCX";
        var stock = await GetStockPrice(symbol, apiKey);

        if (stock != null)
        {
            ViewBag.CurrentStockPrice = stock.Price;
            ViewBag.CurrentStockSymbol = stock.Symbol;
        }
        else
        {
            TempData["Message"] = "Stock not found.";
        }

        // Pass the portfolio back to the view so it can display the existing data
        ViewBag.Balance = _portfolio.Balance;
        ViewBag.Holdings = _portfolio.Holdings;
        return View("Index");
    }

    private async Task<Stock> GetStockPrice(string symbol, string apiKey)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apiKey}");
            var data = JObject.Parse(response);

            if (data["Global Quote"] != null && data["Global Quote"]["05. price"] != null)
            {
                return new Stock
                {
                    Symbol = symbol,
                    Price = decimal.Parse((string)data["Global Quote"]["05. price"])
                };
            }
            else
            {
                // Handle case where stock is not found or data is incomplete
                TempData["Message"] = "Stock not found or data is incomplete.";
                return null;
            }
        }
        catch (Exception ex)
        {
            // Handle any other errors (e.g., network issues)
            TempData["Message"] = $"Error fetching stock data: {ex.Message}";
            return null;
        }
    }

}
