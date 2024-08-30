using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public string AnnualIncome { get; set; }
        public string RiskTolerance { get; set; }
        public string InvestmentExperience { get; set; }
        public string InvestmentGoals { get; set; }

        public void OnGet(
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
            AnnualIncome = annualIncome;
            RiskTolerance = riskTolerance;
            InvestmentExperience = investmentExperience;
            InvestmentGoals = investmentGoals;
        }
    }
}
