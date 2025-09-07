using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.Models;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class SearchInsightsRepository : ISearchInsightsRepository
    {
        private readonly IDbContextProvider _dbContext;

        public SearchInsightsRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<SearchInsight>> GetByCategoriesAsync(Guid searchIndexId,
                                                                           DateTime dateFrom,
                                                                           DateTime dateTo,
                                                                           string dataCategories)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "DateFrom", dateFrom },
                { "DateTo", dateTo },
                { "DataCategories", dataCategories }
            };

            var result = await _dbContext.QueryAsync<SearchInsight>(ConnectionStrings.S2_Search,
                                                                    StoredProcedures.GetSearchInsightsByDataCategories,
                                                                    parameters);

            return result;
        }

        public async Task<IEnumerable<SearchInsight>> GetCountAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "DateFrom", dateFrom },
                { "DateTo", dateTo },
            };

            var result = await _dbContext.QueryAsync<SearchInsight>(ConnectionStrings.S2_Search,
                                                                    StoredProcedures.GetSearchInsightsSearchCountByDateRange,
                                                                    parameters);

            return result;
        }
    }
}
