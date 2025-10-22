using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;

namespace S2Search.Backend.Domain.Interfaces.SearchInsights.Managers
{
    public interface ISearchInsightsManager
    {
        Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints, DateTime dateGenerated);
    }
}
