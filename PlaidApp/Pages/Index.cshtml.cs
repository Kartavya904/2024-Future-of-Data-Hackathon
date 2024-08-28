using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class IndexModel : PageModel
{
    private readonly PlaidService _plaidService;

    public IndexModel(PlaidService plaidService)
    {
        _plaidService = plaidService;
    }

    public string LinkToken { get; private set; }
    public string AccessToken { get; private set; }
    public JArray Accounts { get; private set; }

    public async Task OnGetAsync()
    {
        // Generate the Link Token
        LinkToken = await _plaidService.CreateLinkTokenAsync();

        if (string.IsNullOrEmpty(LinkToken))
        {
            // Log an error or handle the case where the link token was not generated
            Console.WriteLine("Error: Failed to generate link token.");
            // You may want to return an error message or redirect to an error page
        }
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
