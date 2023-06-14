using System.Collections.Generic;

namespace Domain.Models
{
    public class SearchInsightChart
    {
        public string Title { get; set; }
        public IEnumerable<SearchInsight> Data { get; set; }
    }
}
