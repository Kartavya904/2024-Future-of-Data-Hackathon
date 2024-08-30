using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StockWave.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace StockWave.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Passing an empty or default model to avoid null reference errors
            var model = new Dictionary<string, Dictionary<string, string>>();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
