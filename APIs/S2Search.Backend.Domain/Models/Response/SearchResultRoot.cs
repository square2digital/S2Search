using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;

namespace S2Search.Backend.Domain.Models.Response;

public class SearchResultRoot
{
    public SearchProductResult SearchProductResult { get; set; }
    public SearchInsightMessage SearchInsightMessage { get; set; }
}
