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
    public class FeedRepository : IFeedRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;

        public FeedRepository(IDbContextProvider dbContext,
                              ILogger<FeedRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task MergeFeedDocumentsAsync(Guid searchIndexId, IEnumerable<NewFeedDocument> newFeedDocuments)
        {
            try
            {
                var dataTableParameter = ObjectToDataTableMapper.CreateDataTable(newFeedDocuments);
                var parameters = new Dictionary<string, object>()
                {
                    { "SearchIndexId", searchIndexId },
                    { "NewFeedDocuments", dataTableParameter }
                };

                await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore,
                                               StoredProcedures.MergeFeedDocuments,
                                               parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(MergeFeedDocumentsAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetCurrentDocumentIdsAsync(Guid searchIndexId, int pageNumber, int pageSize)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "SearchIndexId", searchIndexId },
                    { "PageNumber", pageNumber },
                    { "PageSize", pageSize }
                };

                var result = await _dbContext.QueryAsync<string>(ConnectionStrings.CustomerResourceStore,
                                                                 StoredProcedures.GetCurrentFeedDocuments,
                                                                 parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCurrentDocumentIdsAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetCurrentDocumentsTotalAsync(Guid searchIndexId)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "SearchIndexId", searchIndexId }
                };

                var result = await _dbContext.QueryFirstOrDefaultAsync<int>(ConnectionStrings.CustomerResourceStore,
                                                                            StoredProcedures.GetCurrentFeedDocumentsTotal,
                                                                            parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCurrentDocumentsTotalAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetDataFormatAsync(Guid customerId, string searchIndexName)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "CustomerId", customerId },
                    { "SearchIndexName", searchIndexName }
                };

                var result = await _dbContext.QuerySingleOrDefaultAsync<string>(ConnectionStrings.CustomerResourceStore,
                                                                                StoredProcedures.GetFeedDataFormat,
                                                                                parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetDataFormatAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<SearchIndexFeedProcessingData> GetSearchIndexFeedProcessingData(Guid customerId, string searchIndexName)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "CustomerId", customerId },
                    { "SearchIndexName", searchIndexName }
                };

                var result = await _dbContext.QueryMultipleAsync<SearchIndexFeedProcessingData>(ConnectionStrings.CustomerResourceStore,
                                                                                            StoredProcedures.GetSearchIndexFeedProcessingData,
                                                                                            parameters);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetSearchIndexFeedProcessingData)} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
