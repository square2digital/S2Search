using System;

namespace S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models
{
    public class SearchInsightDataPoint
    {
        public string DataCategory { get; set; }
        public string DataPoint { get; set; }
        public DateTime Date { get; set; }
    }
}
