using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
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
