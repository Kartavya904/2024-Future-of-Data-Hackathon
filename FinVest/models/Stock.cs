public class Stock
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public decimal PurchasePrice { get; set; }  // New property
    public int Shares { get; set; }
}

namespace FinVest.Models
{
    public class StockDataModel
    {
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public float Volume { get; set; }
    }
}

