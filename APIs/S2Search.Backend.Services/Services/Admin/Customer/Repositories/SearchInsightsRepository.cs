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
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger<SearchInsightsRepository> _logger;
        private readonly string _connectionstring;

        public SearchInsightsRepository(IConfiguration configuration, IDbContextProvider dbContext, ILogger<SearchInsightsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionstring = configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase) ?? throw new InvalidOperationException($"{ConnectionStringKeys.SqlDatabase} connection string not found.");
        }

        public async Task<IEnumerable<SearchInsight>> GetByCategoriesAsync(Guid searchIndexId,
                                                                           DateTime dateFrom,
                                                                           DateTime dateTo,
                                                                           string dataCategories)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "search_index_id", searchIndexId },
                { "date_from", dateFrom },
                { "date_to", dateTo },
                { "data_categories", dataCategories }
            };

            var result = await _dbContext.QueryAsync<SearchInsight>(_connectionstring,
                                                                    StoredProcedures.GetSearchInsightsByDataCategories,
                                                                    parameters);

            return result;
        }

        public async Task<IEnumerable<SearchInsight>> GetCountAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "search_index_id", searchIndexId },
                { "date_from", dateFrom },
                { "date_to", dateTo },
            };

            var result = await _dbContext.QueryAsync<SearchInsight>(_connectionstring,
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
                    { "search_index_id", searchIndexId },
                    { "search_insights_data", dataTableParameter }
                };

                var result = await _dbContext.ExecuteAsync(_connectionstring,
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
                    { "search_index_id", searchIndexId },
                    { "date", date }
                };

                var result = await _dbContext.ExecuteAsync(_connectionstring,
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
