using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly PlaidService _plaidService;

    public IndexModel(PlaidService plaidService)
    {
        _plaidService = plaidService;
    }

    public string LinkToken { get; private set; }

    public async Task OnGet()
    {
        LinkToken = await _plaidService.CreateLinkTokenAsync();
    }
}
