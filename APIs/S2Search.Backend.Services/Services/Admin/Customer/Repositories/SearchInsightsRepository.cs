using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.Models;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.AzureFunctions.FeedServices.Mappers.TinyCsvParser;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class SearchInsightsRepository : ISearchInsightsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger<SearchInsightsRepository> _logger;

        public SearchInsightsRepository(IConfiguration configuration, IDbContextProvider dbContext, ILogger<SearchInsightsRepository> logger)
        {
            _configuration = configuration;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            var result = await _dbContext.QueryAsync<SearchInsight>(ConnectionStrings.SqlDatabase,
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

            var result = await _dbContext.QueryAsync<SearchInsight>(ConnectionStrings.SqlDatabase,
                                                                    StoredProcedures.GetSearchInsightsSearchCountByDateRange,
                                                                    parameters);

            return result;
        }

        public async Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints)
        {
            try
            {
                var dataTableParameter = ObjectToDataTableMapper.CreateDataTable(dataPoints);
                var parameters = new Dictionary<string, object>()
                {
                    { "SearchIndexId", searchIndexId },
                    { "SearchInsightsData", dataTableParameter }
                };

                var result = await _dbContext.ExecuteAsync(ConnectionStrings.SqlDatabase,
                                                           StoredProcedures.AddDataPoints,
                                                           parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(SaveInsightsAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task SaveSearchRequestAsync(Guid searchIndexId, DateTime date)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "SearchIndexId", searchIndexId },
                    { "Date", date }
                };

                var result = await _dbContext.ExecuteAsync(ConnectionStrings.SqlDatabase,
                                                           StoredProcedures.AddSearchRequest,
                                                           parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(SaveInsightsAsync)} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
