using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class StocksController : Controller
{
    public IActionResult Index()
    {
        // Example: Passing the CSV file path to the view
        ViewData["CsvFilePath"] = Url.Content("~/FinVest/data/stocks.csv");

        return View();
    }
}

