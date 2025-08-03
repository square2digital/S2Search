using Domain.Models.Insights;

namespace Domain.Models.Response
{
    public class SearchResultRoot
    {
        public SearchProductResult SearchProductResult { get; set; }
        public SearchInsightMessage SearchInsightMessage { get; set; }
    }
}
