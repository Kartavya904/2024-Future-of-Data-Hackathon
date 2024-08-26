using System;
using System.Net.Http;
using System.Threading.Tasks;

public class PlaidService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public PlaidService(string apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
    }

    public async Task<string> GetAccounts(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.plaid.com/accounts/get");
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = new StringContent($"{{ \"access_token\": \"{accessToken}\" }}");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    // Add more methods for other Plaid API endpoints as needed

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}