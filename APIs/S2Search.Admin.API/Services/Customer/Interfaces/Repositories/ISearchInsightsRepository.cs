using Domain.Customer.Models;

namespace Services.Customer.Interfaces.Repositories
{
    public interface ISearchInsightsRepository
    {
        Task<IEnumerable<SearchInsight>> GetByCategoriesAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo, string dataCategories);
        Task<IEnumerable<SearchInsight>> GetCountAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo);
    }
}
