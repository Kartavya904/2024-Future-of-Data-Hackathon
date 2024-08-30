using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace YourNamespace.Pages
{
    public class _LayoutModel : PageModel
    {
        private readonly UserService _userService;

        public _LayoutModel()
        {
            // Initialize the UserService with MongoDBContext
            _userService = new UserService(new MongoDBContext());
        }

        // Properties to bind data from the form (if using Razor binding)
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        public string Country { get; set; }

        [BindProperty]
        public string Currency { get; set; }

        [BindProperty]
        public decimal AnnualIncome { get; set; }

        [BindProperty]
        public string RiskTolerance { get; set; }

        [BindProperty]
        public string InvestmentExperience { get; set; }

        [BindProperty]
        public string InvestmentGoals { get; set; }

        public void OnPostSignUp()
        {
            // Hash the password (implement your own password hashing mechanism)
            var hashedPassword = HashPassword(Password);

            // Create a new user object
            var newUser = new User
            {
                Email = Email,
                Password = hashedPassword,
                Username = Username,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                DateOfBirth = DateOfBirth,
                Country = Country,
                Currency = Currency,
                AnnualIncome = AnnualIncome,
                RiskTolerance = RiskTolerance,
                InvestmentExperience = InvestmentExperience,
                InvestmentGoals = InvestmentGoals
            };

            // Insert the new user into the database
            _userService.CreateUser(newUser);

            // Set a session or cookie to keep the user logged in
            // (This is a simple example, you'll want to implement proper session management)
            HttpContext.Session.SetString("LoggedInUser", newUser.Email);

            // Redirect to the Finances page or another relevant page
            Response.Redirect("/Finances");
        }

        public IActionResult OnPostLogin()
        {
            // Retrieve user by email
            var user = _userService.GetUserByEmail(Email);

            if (user != null && VerifyPassword(Password, user.Password))
            {
                // Password is correct, log the user in
                HttpContext.Session.SetString("LoggedInUser", user.Email);

                // Redirect to the Finances page
                return RedirectToPage("/Finances");
            }
            else
            {
                // Login failed, show an error message
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // Utility method to hash passwords (use a proper hashing library like BCrypt)
        private string HashPassword(string password)
        {
            // Simple example, replace with real hashing
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        // Utility method to verify passwords
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Simple example, replace with real password verification
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(inputPassword)) == storedPassword;
        }
    }

    // MongoDB context to handle database connection
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext()
        {
            // Connect to MongoDB using the connection string provided
            var client = new MongoClient("mongodb://localhost:27017/");
            
            // Access the specific database 'FinVestDB'
            _database = client.GetDatabase("FinVestDB");
        }

        // Provide access to the 'users' collection within 'FinVestDB'
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }

    // Service class to handle user-related operations
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDBContext context)
        {
            _users = context.Users;
        }

        public void CreateUser(User user)
        {
            _users.InsertOne(user);
        }

        public User GetUserByEmail(string email)
        {
            return _users.Find(user => user.Email == email).FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            _users.ReplaceOne(u => u.Id == user.Id, user);
        }
    }

    // User model representing the user data
    public class User
    {
        public ObjectId Id { get; set; }  // MongoDB automatically assigns an ObjectId
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public decimal AnnualIncome { get; set; }
        public string RiskTolerance { get; set; }
        public string InvestmentExperience { get; set; }
        public string InvestmentGoals { get; set; }
    }
}
