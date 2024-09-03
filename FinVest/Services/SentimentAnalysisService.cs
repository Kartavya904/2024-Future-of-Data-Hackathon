using Azure;
using Azure.AI.TextAnalytics;
using System;

public class SentimentAnalysisService
{
    private readonly TextAnalyticsClient _client;

    public SentimentAnalysisService(string endpoint, string apiKey)
    {
        var credentials = new AzureKeyCredential(apiKey);
        _client = new TextAnalyticsClient(new Uri(endpoint), credentials);
    }

    public string AnalyzeSentiment(string text)
    {
        DocumentSentiment sentiment = _client.AnalyzeSentiment(text);
        return sentiment.Sentiment.ToString();
    }
}
