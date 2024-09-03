public class UserPortfolio
{
    public decimal Balance { get; set; } = 100000;
    public Dictionary<string, Stock> Holdings { get; set; } = new Dictionary<string, Stock>();

    public decimal GetProfitMargin(string symbol, decimal currentPrice)
    {
        if (Holdings.ContainsKey(symbol))
        {
            var stock = Holdings[symbol];
            return (currentPrice - stock.PurchasePrice) * stock.Shares;
        }
        return 0;
    }
}
