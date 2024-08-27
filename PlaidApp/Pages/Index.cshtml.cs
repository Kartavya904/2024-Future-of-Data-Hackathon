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
    }

    public async Task OnPostAsync(string publicToken)
    {
        // Exchange public token for access token
        AccessToken = await _plaidService.ExchangePublicTokenForAccessTokenAsync(publicToken);

        // Retrieve account information
        Accounts = await _plaidService.GetAccountInfoAsync(AccessToken);
    }
}
