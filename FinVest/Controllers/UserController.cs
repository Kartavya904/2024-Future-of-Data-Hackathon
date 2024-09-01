// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;
// using FinVest.Data; // Replace with your actual namespace
// using FinVest.Models; // Replace with your actual namespace
// using Microsoft.EntityFrameworkCore;

// [Route("api/[controller]")]
// [ApiController]
// public class UserController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;

//     public UserController(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     [HttpPost]
//     [Route("SaveUserInfo")]
//     public async Task<IActionResult> SaveUserInfo([FromBody] UserInfoModel model)
//     {
//         if (ModelState.IsValid)
//         {
//             // Ensure email is unique
//             if (await _context.Users.AnyAsync(u => u.Email == model.Email))
//             {
//                 return BadRequest("Email already exists.");
//             }

//             var user = new User
//             {
//                 Username = model.Username,
//                 FullName = model.FullName,
//                 Email = model.Email,
//                 Password = BCrypt.Net.BCrypt.HashPassword(model.Password), // Hash the password before saving
//                 PhoneNumber = model.PhoneNumber,
//                 DateOfBirth = model.DateOfBirth,
//                 Country = model.Country,
//                 Currency = model.Currency,
//                 AnnualIncome = model.AnnualIncome,
//                 RiskTolerance = model.RiskTolerance,
//                 InvestmentExperience = model.InvestmentExperience,
//                 InvestmentGoals = model.InvestmentGoals
//             };

//             // _context.Users.Add(user);
//             // await _context.SaveChangesAsync();

//             return Ok(new { success = true });
//         }

//         return BadRequest("Invalid data.");
//     }
// }
