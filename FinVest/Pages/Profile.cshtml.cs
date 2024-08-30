using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinVest.Pages
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public DateTime DateOfBirth { get; set; }
        [BindProperty]
        public string Country { get; set; }
        [BindProperty]
        public string Currency { get; set; }
        [BindProperty]
        public string AnnualIncome { get; set; }
        [BindProperty]
        public string RiskTolerance { get; set; }
        [BindProperty]
        public string InvestmentExperience { get; set; }
        [BindProperty]
        public string InvestmentGoals { get; set; }

        public void OnGet()
        {
            // Populate with user data
            Username = "JohnDoe";
            FullName = "John Doe";
            Email = "johndoe@example.com";
            PhoneNumber = "123-456-7890";
            DateOfBirth = new DateTime(1990, 1, 1);
            Country = "USA";
            Currency = "USD - US Dollar";
            AnnualIncome = "$50,000 - $100,000";
            RiskTolerance = "Medium";
            InvestmentExperience = "Intermediate";
            InvestmentGoals = "Retirement planning";
        }

        public IActionResult OnPost()
        {
            // Update profile logic here
            return RedirectToPage("/Finances");
        }
    }
}
