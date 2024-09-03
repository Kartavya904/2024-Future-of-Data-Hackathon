using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FinVest.Pages
{
    public class FinancesModel : PageModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; } 
        public string CurrencySymbol { get; set; } // Added CurrencySymbol
        public string AnnualIncome { get; set; }
        public string RiskTolerance { get; set; }
        public string InvestmentExperience { get; set; }
        public string InvestmentGoals { get; set; }

        private readonly PlaidService _plaidService;
        public string LinkToken { get; private set; }
        public string AccessToken { get; private set; }
        public JArray Accounts { get; private set; }

        public double IncomeMultiplier { get; set; } = 1;
        public double CurrencyMultiplier { get; set; } = 1; 
        public string CheckingsAccount { get; set; } = "0000 0000 0000 0000";
        public string SavingsAccount { get; set; } = "1111 1111 1111 1111";

        public List<Transaction> TransactionHistory { get; set; }
        public List<Transaction> TransactionHistoryMonth { get; set; }
        public List<Transaction> TransactionHistoryYear { get; set; }

        public int TotalTransactions { get; private set; }
        public double TotalExpenditure { get; private set; }
        public double TotalIncome { get; private set; }

        public class Transaction
        {
            public string TransactionId { get; set; }
            public DateTime TransactionDate { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public double Amount { get; set; }
            public double RunningBalance { get; set; }
        }

        public FinancesModel(PlaidService plaidService)
        {
            _plaidService = plaidService;
        }

        public async Task OnGet(
            string username, 
            string fullName, 
            string email, 
            string phoneNumber, 
            DateTime dateOfBirth, 
            string country, 
            string currency, 
            string annualIncome, 
            string riskTolerance, 
            string investmentExperience, 
            string investmentGoals)
        {
            Username = username;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Country = country;
            Currency = currency;
            // Determine CurrencySymbol and CurrencyMultiplier based on selected currency
            (CurrencySymbol, CurrencyMultiplier) = currency switch
            {
                "USD" => ("$", 1.00),
                "EUR" => ("€", 0.85),
                "INR" => ("₹", 75.00),
                "GBP" => ("£", 0.75),
                "JPY" => ("¥", 110.00),
                "CNY" => ("¥", 6.45),
                "RUB" => ("₽", 73.50),
                "BRL" => ("R$", 5.20),
                "AUD" => ("A$", 1.35),
                "CAD" => ("C$", 1.25),
                _ => ("$", 1.00),
            };
            AnnualIncome = annualIncome;
            // Determine IncomeMultiplier based on selected annual income
            IncomeMultiplier = annualIncome switch
            {
                "Less than $5,000" => 0.05,
                "$5,000 - $10,000" => 0.10,
                "$10,000 - $25,000" => 0.175,
                "$25,000 - $100,000" => 0.625,
                "More than $100,000" => 1.00,
                _ => 1.00,
            };
            RiskTolerance = riskTolerance;
            InvestmentExperience = investmentExperience;
            InvestmentGoals = investmentGoals;

            LinkToken = await _plaidService.CreateLinkTokenAsync();

            if (string.IsNullOrEmpty(LinkToken))
            {
                Console.WriteLine("Error: Failed to retrieve Link token.");
            }

            TransactionHistory = GenerateTransactionHistory();
            TransactionHistoryMonth = GenerateMonthlyTransactionHistory(TransactionHistory);
            TransactionHistoryYear = GenerateYearlyTransactionHistory(TransactionHistory);

            CalculateTotals();
        }

        private List<Transaction> GenerateTransactionHistory()
        {
            var random = new Random();
            var transactionHistory = new List<Transaction>();
            double runningBalance = 0;
            var categories = new List<string> { "Groceries", "Rent", "Utilities", "Entertainment", "Dining", "Investment", "Shopping", "Travel" };

            for (int year = 2016; year <= 2024; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    if (year == 2024 && month > DateTime.Now.Month)
                        break;

                    int numDeposits = random.Next(2, 5);
                    int numWithdrawals = 0;
                    if (numDeposits == 2) { numWithdrawals = 3; }
                    else if (numDeposits == 3) { numWithdrawals = random.Next(4, 6); }
                    else if (numDeposits == 4) { numWithdrawals = 6; }

                    for (int i = 0; i < numDeposits; i++)
                    {
                        var amount = random.Next(2000, 5000) * IncomeMultiplier * CurrencyMultiplier;
                        runningBalance += amount;

                        var transaction = new Transaction
                        {
                            TransactionId = random.Next(10000000, 99999999).ToString(),
                            TransactionDate = new DateTime(year, month, random.Next(1, 5)),
                            Description = "Deposit",
                            Category = "Salary",
                            Amount = amount,
                            RunningBalance = runningBalance
                        };
                        transactionHistory.Add(transaction);
                    }

                    for (int i = 0; i < numWithdrawals; i++)
                    {
                        var amount = random.Next(1000, 3000) * IncomeMultiplier * CurrencyMultiplier;

                        if (runningBalance - amount >= 0)
                        {
                            runningBalance -= amount;
                            var transaction = new Transaction
                            {
                                TransactionId = random.Next(10000000, 99999999).ToString(),
                                TransactionDate = new DateTime(year, month, random.Next(5, 28)),
                                Description = "Withdrawal",
                                Category = categories[random.Next(categories.Count)],
                                Amount = -amount,
                                RunningBalance = runningBalance
                            };
                            transactionHistory.Add(transaction);
                        }
                    }
                }
            }

            return transactionHistory;
        }

        private List<Transaction> GenerateMonthlyTransactionHistory(List<Transaction> transactions)
        {
            var monthlyTransactionHistory = new List<Transaction>();
            double runningBalance = 0;

            var groupedTransactions = transactions.GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
                                                  .Select(g => new
                                                  {
                                                      g.Key,
                                                      Deposits = g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                                                      Withdrawals = g.Where(t => t.Amount < 0).Sum(t => t.Amount)
                                                  });

            foreach (var month in groupedTransactions)
            {
                runningBalance += month.Deposits + month.Withdrawals; // Update running balance

                monthlyTransactionHistory.Add(new Transaction
                {
                    TransactionId = $"Month-{month.Key.Month}-Deposit",
                    TransactionDate = new DateTime(month.Key.Year, month.Key.Month, 1),
                    Description = "Monthly Deposits",
                    Category = "Salary",
                    Amount = month.Deposits,
                    RunningBalance = runningBalance
                });

                monthlyTransactionHistory.Add(new Transaction
                {
                    TransactionId = $"Month-{month.Key.Month}-Withdrawal",
                    TransactionDate = new DateTime(month.Key.Year, month.Key.Month, 1),
                    Description = "Monthly Withdrawals",
                    Category = "Expenses",
                    Amount = month.Withdrawals,
                    RunningBalance = runningBalance
                });
            }

            return monthlyTransactionHistory;
        }

        private List<Transaction> GenerateYearlyTransactionHistory(List<Transaction> transactions)
        {
            var yearlyTransactionHistory = new List<Transaction>();
            double runningBalance = 0;

            var groupedTransactions = transactions.GroupBy(t => t.TransactionDate.Year)
                                                  .Select(g => new
                                                  {
                                                      Year = g.Key,
                                                      Deposits = g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                                                      Withdrawals = g.Where(t => t.Amount < 0).Sum(t => t.Amount)
                                                  });

            foreach (var year in groupedTransactions)
            {
                runningBalance += year.Deposits + year.Withdrawals; // Update running balance

                yearlyTransactionHistory.Add(new Transaction
                {
                    TransactionId = $"Year-{year.Year}-Deposit",
                    TransactionDate = new DateTime(year.Year, 1, 1),
                    Description = "Yearly Deposits",
                    Category = "Salary",
                    Amount = year.Deposits,
                    RunningBalance = runningBalance
                });

                yearlyTransactionHistory.Add(new Transaction
                {
                    TransactionId = $"Year-{year.Year}-Withdrawal",
                    TransactionDate = new DateTime(year.Year, 1, 1),
                    Description = "Yearly Withdrawals",
                    Category = "Expenses",
                    Amount = year.Withdrawals,
                    RunningBalance = runningBalance
                });
            }

            return yearlyTransactionHistory;
        }

        private void CalculateTotals()
        {
            TotalTransactions = TransactionHistory.Count;
            TotalExpenditure = TransactionHistory.Where(t => t.Amount < 0).Sum(t => -t.Amount);
            TotalIncome = TransactionHistory.Where(t => t.Amount > 0).Sum(t => t.Amount);
        }

        public async Task<IActionResult> OnPostAsync(string publicToken)
        {
            if (string.IsNullOrEmpty(publicToken))
            {
                Console.WriteLine("Error: Public token is null or empty.");
                return BadRequest("Public token is required.");
            }

            try
            {
                AccessToken = await _plaidService.ExchangePublicTokenForAccessTokenAsync(publicToken);

                if (string.IsNullOrEmpty(AccessToken))
                {
                    Console.WriteLine("Error: Failed to retrieve access token.");
                    return StatusCode(500, "Failed to retrieve access token.");
                }

                Accounts = await _plaidService.GetAccountInfoAsync(AccessToken);

                if (Accounts == null || Accounts.Count == 0)
                {
                    Console.WriteLine("Error: No accounts retrieved.");
                    return StatusCode(500, "Failed to retrieve accounts.");
                }

            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }

            return Page();
        }
    }
}