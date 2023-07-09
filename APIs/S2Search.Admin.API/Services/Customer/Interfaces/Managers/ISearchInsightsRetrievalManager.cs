using Domain.Customer.Models;

namespace Services.Customer.Interfaces.Managers
{
    public interface ISearchInsightsRetrievalManager
    {
        Task<SearchInsightChart> GetChartByNameAsync(Guid searchIndexId,
                                                            DateTime dateFrom,
                                                            DateTime dateTo,
                                                            string reportName);
        Task<SearchInsightTile> GetTileByNameAsync(Guid searchIndexId,
                                                           DateTime dateFrom,
                                                           DateTime dateTo,
                                                           string reportName,
                                                           bool includePreviousPeriod);
        Task<SearchInsightSummary> GetSummaryAsync(Guid searchIndexId);
    }
}
