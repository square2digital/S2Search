using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;

namespace S2Search.Backend.Domain.Interfaces.SearchInsights.Managers
{
    public interface IDataPointsExtractionManager
    {
        IEnumerable<SearchInsightDataPoint> Extract(SearchInsightMessage searchInsightMessage);
    }
}
