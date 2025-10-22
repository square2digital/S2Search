using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;

namespace S2Search.Backend.Domain.Interfaces.SearchInsights.Repositories
{
    public interface ISearchInsightsRepository
    {
        Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints);
        Task SaveSearchRequestAsync(Guid searchIndexId, DateTime date);
    }
}
