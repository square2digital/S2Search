using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Repositories;
using S2Search.Backend.Services.SearchInsights.Mappers;

namespace S2Search.Backend.Services.SearchInsights.Repositories
{
    public class SearchInsightsRepository : ISearchInsightsRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;

        public SearchInsightsRepository(IDbContextProvider dbContext,
                                        ILogger<SearchInsightsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                var result = await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore,
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

                var result = await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore,
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
