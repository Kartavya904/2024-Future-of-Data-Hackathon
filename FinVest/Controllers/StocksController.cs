using Microsoft.AspNetCore.Mvc;
using FinVest.Services;
using FinVest.Models;

namespace FinVest.Controllers
{
    public class StocksController : Controller
    {
        private readonly PredictiveAnalysisService _predictiveAnalysisService;

        public StocksController()
        {
            _predictiveAnalysisService = new PredictiveAnalysisService();
            _predictiveAnalysisService.TrainModel(@"C:\Users\skhar\Downloads\2024-Future-of-Data-Hackathon\FinVest\data\apple.csv");
        }

        [HttpGet]
        public IActionResult Predict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(StockDataModel stockData)
        {
            float predictedClosePrice = _predictiveAnalysisService.Predict(stockData);
            ViewBag.PredictedClosePrice = predictedClosePrice;
            return View(stockData);
        }
    }
}
