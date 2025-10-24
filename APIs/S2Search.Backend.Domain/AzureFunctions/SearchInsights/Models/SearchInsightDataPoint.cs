using System;
using System.Text.Json.Serialization;

namespace S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models
{
    public class SearchInsightDataPoint
    {

        [JsonPropertyName("data_category")]
        public string DataCategory { get; set; }

        [JsonPropertyName("data_point")]
        public string DataPoint { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

    }
}
