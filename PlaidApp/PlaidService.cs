using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class PlaidService
{
    private readonly string clientId = "66cbdb593ed8de001975daf7";
    private readonly string secret = "0c58732fb84497c0993bfe0fea28d1";
    private readonly string plaidEnvironment = "https://sandbox.plaid.com"; // Change to production URL if needed

    public async Task<string> CreateLinkTokenAsync()
    {
        var client = new RestClient(plaidEnvironment);
        var request = new RestRequest("/link/token/create", Method.Post);
        request.AddJsonBody(new
        {
            client_id = clientId,
            secret = secret,
            client_name = "Your App Name",
            country_codes = new[] { "US" },
            language = "en",
            user = new
            {
                client_user_id = "unique_user_id" // Generate a unique user ID for your users
            },
            products = new[] { "auth", "transactions" }
        });

        RestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var jsonResponse = JObject.Parse(response.Content);
            return jsonResponse["link_token"]?.ToString();
        }
        else
        {
            // Handle errors, log the issue, or throw an exception with detailed information
            var errorResponse = JObject.Parse(response.Content);
            string errorMessage = errorResponse["error_message"]?.ToString();
            throw new ApplicationException($"Error creating link token: {errorMessage}");
        }
    }
}
