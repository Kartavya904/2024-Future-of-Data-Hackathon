using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StockWave.Models
{
    public class MetaData
    {
        [JsonPropertyName("1. Information")]
        public string Information { get; set; }

        [JsonPropertyName("2. Symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("3. Last Refreshed")]
        public string LastRefreshed { get; set; }

        [JsonPropertyName("4. Output Size")]
        public string OutputSize { get; set; }

        [JsonPropertyName("5. Time Zone")]
        public string TimeZone { get; set; }
    }

    public class TimeSeriesData
    {
        [JsonPropertyName("1. open")]
        public string Open { get; set; }

        [JsonPropertyName("2. high")]
        public string High { get; set; }

        [JsonPropertyName("3. low")]
        public string Low { get; set; }

        [JsonPropertyName("4. close")]
        public string Close { get; set; }

        [JsonPropertyName("5. volume")]
        public string Volume { get; set; }
    }

    public class AlphaVantageResponse
    {
        [JsonPropertyName("Meta Data")]
        public MetaData MetaData { get; set; }

        [JsonPropertyName("Time Series (Daily)")]
        public Dictionary<string, TimeSeriesData> TimeSeriesDaily { get; set; }
    }
}
