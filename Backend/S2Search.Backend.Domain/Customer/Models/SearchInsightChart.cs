using System.Collections.Generic;

namespace S2Search.Backend.Domain.Customer.Models
{
    public class SearchInsightChart
    {
        public string Title { get; set; }
        public IEnumerable<SearchInsight> Data { get; set; }
    }
}
