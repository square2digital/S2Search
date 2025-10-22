using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Logging;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using Services.Mappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
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
