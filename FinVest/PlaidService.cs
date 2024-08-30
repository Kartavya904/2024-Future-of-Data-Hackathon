using RestSharp;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class PlaidService
{
    private readonly string clientId = "66cbdb593ed8de001975daf7";
    private readonly string secret = "0c58732fb84497c0993bfe0fea28d1";
    private readonly string plaidEnvironment = "https://sandbox.plaid.com"; // Change to production URL if needed

    public async Task<string?> CreateLinkTokenAsync()
    {
        var client = new RestClient(plaidEnvironment);
        var request = new RestRequest("/link/token/create", Method.Post);
        request.AddJsonBody(new
        {
            client_id = clientId,
            secret = secret,
            client_name = "FinVest",
            country_codes = new[] { "US" },
            language = "en",
            user = new
            {
                client_user_id = "1234567" // Generate a unique user ID for your users
            },
            products = new[] { "auth", "transactions" }
        });

        RestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var jsonResponse = JObject.Parse(response.Content!);
            return jsonResponse["link_token"]?.ToString();
        }
        else
        {
            var errorMessage = "Error creating link token.";
            if (!string.IsNullOrEmpty(response.Content))
            {
                var errorResponse = JObject.Parse(response.Content!);
                errorMessage = errorResponse["error_message"]?.ToString() ?? errorMessage;
            }
            throw new ApplicationException(errorMessage);
        }
    }

    public async Task<string?> ExchangePublicTokenForAccessTokenAsync(string publicToken)
    {
        // Validate the public token
        if (string.IsNullOrWhiteSpace(publicToken))
        {
            throw new ArgumentException("Public token must be a non-empty string", nameof(publicToken));
        }

        // Log the public token for debugging purposes (consider masking this in production)
        Console.WriteLine($"Public Token: {publicToken}");

        var client = new RestClient(plaidEnvironment);
        var request = new RestRequest("/item/public_token/exchange", Method.Post);
        request.AddJsonBody(new
        {
            client_id = clientId,
            secret = secret,
            public_token = publicToken
        });

        RestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var jsonResponse = JObject.Parse(response.Content!);
            var accessToken = jsonResponse["access_token"]?.ToString();

            // Log the access token for debugging (consider masking this in production)
            Console.WriteLine($"Access Token: {accessToken}");

            return accessToken;
        }
        else
        {
            var errorMessage = "Error exchanging public token.";
            if (!string.IsNullOrEmpty(response.Content))
            {
                var errorResponse = JObject.Parse(response.Content!);
                errorMessage = errorResponse["error_message"]?.ToString() ?? errorMessage;
            }
            throw new ApplicationException(errorMessage);
        }
    }

    public async Task<JArray> GetAccountInfoAsync(string accessToken)
    {
        // Validate the access token
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token must be a non-empty string", nameof(accessToken));
        }

        var client = new RestClient(plaidEnvironment);
        var request = new RestRequest("/auth/get", Method.Post);
        request.AddJsonBody(new
        {
            client_id = clientId,
            secret = secret,
            access_token = accessToken
        });

        RestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var jsonResponse = JObject.Parse(response.Content!);
            var accounts = (JArray?)jsonResponse["accounts"];

            if (accounts == null)
            {
                throw new ApplicationException("No accounts were retrieved.");
            }

            // Log the number of accounts retrieved
            Console.WriteLine($"Number of accounts retrieved: {accounts.Count}");

            return accounts;
        }
        else
        {
            var errorMessage = "Error retrieving account information.";
            if (!string.IsNullOrEmpty(response.Content))
            {
                var errorResponse = JObject.Parse(response.Content!);
                errorMessage = errorResponse["error_message"]?.ToString() ?? errorMessage;
            }
            throw new ApplicationException(errorMessage);
        }
    }
}