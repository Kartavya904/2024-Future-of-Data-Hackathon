using RestSharp;
using System;
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
            client_name = "Kartavya",
            country_codes = new[] { "US" },
            language = "en",
            user = new
            {
                client_user_id = "1234567" // Generate a unique user ID for your users
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
            var errorResponse = JObject.Parse(response.Content);
            string errorMessage = errorResponse["error_message"]?.ToString();
            throw new ApplicationException($"Error creating link token: {errorMessage}");
        }
    }

    public async Task<string> ExchangePublicTokenForAccessTokenAsync(string publicToken)
    {
        var client = new RestClient(plaidEnvironment);
        var request = new RestRequest("/item/public_token/exchange", Method.Post);
        request.AddJsonBody(new
        {
            client_id = clientId,
            secret = secret,
            public_token = publicToken
        });

        RestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var jsonResponse = JObject.Parse(response.Content);
            return jsonResponse["access_token"]?.ToString();
        }
        else
        {
            var errorResponse = JObject.Parse(response.Content);
            string errorMessage = errorResponse["error_message"]?.ToString();
            throw new ApplicationException($"Error exchanging public token: {errorMessage}");
        }
    }

    public async Task<string> GetUserNameAsync(string accessToken)
    {
        var client = new RestClient(plaidEnvironment);
        var request = new RestRequest("/auth/get", Method.Post);
        request.AddJsonBody(new
        {
            client_id = clientId,
            secret = secret,
            access_token = accessToken
        });

        RestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var jsonResponse = JObject.Parse(response.Content);
            var accounts = jsonResponse["accounts"];
            string userName = accounts[0]["name"]?.ToString(); // Assuming the first account is sufficient for this example
            return userName;
        }
        else
        {
            var errorResponse = JObject.Parse(response.Content);
            string errorMessage = errorResponse["error_message"]?.ToString();
            throw new ApplicationException($"Error retrieving user name: {errorMessage}");
        }
    }
}

public partial class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            PlaidService plaidService = new PlaidService();

            // Step 1: Create a Link Token
            string linkToken = await plaidService.CreateLinkTokenAsync();
            Console.WriteLine($"Link Token: {linkToken}");

            // Simulate obtaining the public token (you would get this from the Plaid Link flow in a real application)
            string publicToken = "PUBLIC_TOKEN_FROM_PLAID_LINK"; // Replace with actual public token

            // Step 2: Exchange the public token for an access token
            string accessToken = await plaidService.ExchangePublicTokenForAccessTokenAsync(publicToken);
            Console.WriteLine($"Access Token: {accessToken}");

            // Step 3: Retrieve and display the user's name
            string userName = await plaidService.GetUserNameAsync(accessToken);
            Console.WriteLine($"User Name: {userName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
