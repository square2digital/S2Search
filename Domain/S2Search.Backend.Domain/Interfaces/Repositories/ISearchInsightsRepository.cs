using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Customer.Models;
namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface ISearchInsightsRepository
{
    Task<IEnumerable<SearchInsight>> GetByCategoriesAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo, string dataCategories);
    Task<IEnumerable<SearchInsight>> GetCountAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo);
    Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints);
    Task SaveSearchRequestAsync(Guid searchIndexId, DateTime date);
}
