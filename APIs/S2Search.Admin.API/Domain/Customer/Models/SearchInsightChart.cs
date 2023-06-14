using System.Collections.Generic;

namespace Domain.Customer.Models
{
    public class SearchInsightChart
    {
        public string Title { get; set; }
        public IEnumerable<SearchInsight> Data { get; set; }
    }
}
