using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Domain.Models;
using S2Search.Backend.Services.AzureFunctions.FeedServices.Mappers.TinyCsvParser;
using Services.Interfaces.Repositories;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Repositories
{
    public class FeedServicesRepository : IFeedServicesRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;
        private readonly string _connectionstring;

        public FeedServicesRepository(IConfiguration configuration, IDbContextProvider dbContext, ILogger<FeedServicesRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionstring = configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase) ?? throw new InvalidOperationException($"{ConnectionStringKeys.SqlDatabase} connection string not found.");
        }

        public async Task MergeFeedDocumentsAsync(Guid searchIndexId, IEnumerable<NewFeedDocument> newFeedDocuments)
        {
            try
            {
                var dataTableParameter = ObjectToDataTableMapper.CreateDataTable(newFeedDocuments);
                var parameters = new Dictionary<string, object>()
                {
                    { "search_index_id", searchIndexId },
                    { "new_feed_documents", dataTableParameter }
                };

                await _dbContext.ExecuteAsync(_connectionstring,
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
                    { "search_index_id", searchIndexId },
                    { "page_number", pageNumber },
                    { "page_size", pageSize }
                };

                var result = await _dbContext.QueryAsync<string>(_connectionstring,
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
                    { "search_index_id", searchIndexId }
                };

                var result = await _dbContext.QueryFirstOrDefaultAsync<int>(_connectionstring,
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
                    { "customer_id", customerId },
                    { "search_index_name", searchIndexName }
                };

                var result = await _dbContext.QuerySingleOrDefaultAsync<string>(_connectionstring,
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
                    { "customer_id", customerId },
                    { "search_index_name", searchIndexName }
                };

                var result = await _dbContext.QueryMultipleAsync<SearchIndexFeedProcessingData>(_connectionstring,
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
