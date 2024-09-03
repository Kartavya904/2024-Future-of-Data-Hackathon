public class Stock
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public decimal PurchasePrice { get; set; }  // New property
    public int Shares { get; set; }
}
