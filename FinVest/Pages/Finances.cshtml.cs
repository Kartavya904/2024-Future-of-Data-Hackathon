using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string Currency { get; set; } // This remains unchanged as it's the user's preferred currency
        public string AnnualIncome { get; set; }
        public string RiskTolerance { get; set; }
        public string InvestmentExperience { get; set; }
        public string InvestmentGoals { get; set; }

        // Plaid Implementation
        private readonly PlaidService _plaidService;
        public string LinkToken { get; private set; }
        public string AccessToken { get; private set; }
        public JArray Accounts { get; private set; }

        public double IncomeMultiplier { get; set; } = 1;
        public double CurrencyMultiplier { get; set; } = 1; // Renamed from Currency to CurrencyMultiplier
        public string CheckingsAccount { get; set; } = "0000 0000 0000 0000";
        public string SavingsAccount { get; set; } = "1111 1111 1111 1111";

        public List<Transaction> TransactionHistory { get; set; }

        // New properties for totals
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
            // Assign user information parameters to properties
            Username = username;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Country = country;
            Currency = currency;
            AnnualIncome = annualIncome;
            RiskTolerance = riskTolerance;
            InvestmentExperience = investmentExperience;
            InvestmentGoals = investmentGoals;

            // Generate the Plaid Link Token
            LinkToken = await _plaidService.CreateLinkTokenAsync();

            if (string.IsNullOrEmpty(LinkToken))
            {
                Console.WriteLine("Error: Failed to retrieve Link token.");
            }

            // Generate transaction history
            TransactionHistory = GenerateTransactionHistory();

            // Calculate totals
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
                    if (numDeposits == 2) {numWithdrawals = 3;}
                    else if (numDeposits == 3) {numWithdrawals = random.Next(4,6);}
                    else if (numDeposits == 4) {numWithdrawals = 6;}                    

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

                        // Check if withdrawal would cause negative balance
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
                // Log an error or handle the case where the public token is missing
                Console.WriteLine("Error: Public token is null or empty.");
                return BadRequest("Public token is required.");
            }

            try
            {
                // Exchange public token for access token
                AccessToken = await _plaidService.ExchangePublicTokenForAccessTokenAsync(publicToken);

                if (string.IsNullOrEmpty(AccessToken))
                {
                    // Handle the case where access token is not returned
                    Console.WriteLine("Error: Failed to retrieve access token.");
                    return StatusCode(500, "Failed to retrieve access token.");
                }

                // Retrieve account information
                Accounts = await _plaidService.GetAccountInfoAsync(AccessToken);

                if (Accounts == null || Accounts.Count == 0)
                {
                    // Handle the case where no accounts are returned
                    Console.WriteLine("Error: No accounts retrieved.");
                    return StatusCode(500, "Failed to retrieve accounts.");
                }

                // Optionally, you could return a view or JSON result with the retrieved accounts
                // return new JsonResult(Accounts);

            }
            catch (ApplicationException ex)
            {
                // Log and handle application-specific exceptions
                Console.WriteLine($"Application error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                // Log and handle unexpected exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }

            // If everything is successful, return the default page
            return Page();
        }
    }
}
