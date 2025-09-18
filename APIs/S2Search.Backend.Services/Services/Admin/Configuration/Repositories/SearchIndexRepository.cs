using Domain.Customer.Enums;
using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Configuration.SearchResources.Synonyms;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.CustomerPricing;
using S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;
using S2Search.Backend.Domain.Customer.SearchResources.SearchInstanceKeys;
using S2Search.Backend.Domain.Customer.Shared;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Services.Admin.Configuration.Repositories
{
    public class SearchIndexRepository : ISearchIndexRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly IQueueManager? _queueManager;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        // Only one public constructor for DI
        public SearchIndexRepository(IDbContextProvider dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = configuration.GetConnectionString("S2_Search");
        }


        public async Task<SearchIndexQueryCredentials> GetQueryCredentialsAsync(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexQueryCredentials>(
                _connectionString,
                StoredProcedures.GetSearchIndexQueryCredentials,
                parameters);

            return result;
        }

        public async Task<IEnumerable<GenericSynonyms>> GetGenericSynonymsByCategoryAsync(string category)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "Category", category }
            };

            var result = await _dbContext.QueryAsync<GenericSynonyms>(_connectionString,
                                                                                 StoredProcedures.GetGenericSynonymsByCategory,
                                                                                 parameters);

            return result;
        }

        public void Create(SearchIndexRequest resourceRequest)
        {
            var message = _queueManager.CreateMessage(SearchIndexQueues.Create, resourceRequest);
            _queueManager.EnqueueMessageAsync(message);
        }

        public async Task CreateAsync(SearchIndexRequest resourceRequest)
        {
            var message = _queueManager.CreateMessage(SearchIndexQueues.Create, resourceRequest);
            await _queueManager.EnqueueMessageAsync(message);
        }

        public async Task CreateKeysAsync(SearchIndexKeyGenerationRequest keyGenerationRequest)
        {
            var searchIndex = await GetAsync(keyGenerationRequest.CustomerId, keyGenerationRequest.SearchIndexId);

            if (!searchIndex.SearchInstanceId.HasValue)
            {
                throw new Exception($"{nameof(searchIndex.SearchInstanceId)} is null");
            }

            var SearchInstanceKeyGenerationRequest = ConvertToSearchInstanceKeyGenerationRequest(keyGenerationRequest, searchIndex);

            var message = _queueManager.CreateMessage(SearchIndexQueues.CreateKeys, SearchInstanceKeyGenerationRequest);
            await _queueManager.EnqueueMessageAsync(message);
        }

        private static SearchInstanceKeyGenerationRequest ConvertToSearchInstanceKeyGenerationRequest(SearchIndexKeyGenerationRequest keyGenerationRequest, SearchIndex searchIndex)
        {
            return new SearchInstanceKeyGenerationRequest()
            {
                SearchIndexId = searchIndex.SearchIndexId,
                SearchInstanceId = searchIndex.SearchInstanceId.Value,
                KeysToGenerate = keyGenerationRequest.KeysToGenerate.Select(keyName => new SearchInstanceKeyRequest()
                {
                    Name = keyName,
                    Type = SearchInstanceKeyType.Query
                })
            };
        }

        private static SearchInstanceQueryKeyDeletionRequest ConvertToSearchInstanceQueryKeyDeletionRequest(SearchIndexKeyDeletionRequest keyGenerationRequest, SearchIndex searchIndex)
        {
            return new SearchInstanceQueryKeyDeletionRequest()
            {
                SearchIndexId = keyGenerationRequest.SearchIndexId,
                SearchInstanceId = searchIndex.SearchInstanceId.Value,
                KeysToDelete = keyGenerationRequest.KeysToDelete.Select(keyToDelete => new QueryKey()
                {
                    Name = keyToDelete.Name,
                    ApiKey = keyToDelete.ApiKey
                })
            };
        }

        public void Delete(Guid searchIndexId)
        {
            var message = _queueManager.CreateMessage(SearchIndexQueues.Delete, searchIndexId);
            _queueManager.EnqueueMessageAsync(message);
        }

        public async Task DeleteAsync(Guid searchIndexId)
        {
            var message = _queueManager.CreateMessage(SearchIndexQueues.Delete, searchIndexId);
            await _queueManager.EnqueueMessageAsync(message);
        }

        public async Task<SearchIndex> GetAsync(Guid customerId, Guid searchIndexId)
        {
            object parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "CustomerId", customerId }
            };

            try
            {
                var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndex>(_connectionString,
                                                                                     StoredProcedures.GetSearchIndex,
                                                                                     parameters);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<SearchIndexFull> GetFullAsync(Guid customerId, Guid searchIndexId)
        {
            object parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "CustomerId", customerId }
            };

            try
            {
                var result = await _dbContext.QueryMultipleAsync<SearchIndexFull>(_connectionString,
                                                                                  StoredProcedures.GetSearchIndexFull,
                                                                                  parameters);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<SearchIndexKeys>> GetKeysAsync(Guid customerId, Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QueryAsync<SearchIndexKeys>(_connectionString,
                                                                      StoredProcedures.GetSearchIndexKeysForCustomer,
                                                                      parameters);

            return result;
        }

        public async Task DeleteKeysAsync(SearchIndexKeyDeletionRequest keyDeletionRequest)
        {
            var searchIndex = await GetAsync(keyDeletionRequest.CustomerId, keyDeletionRequest.SearchIndexId);

            if (!searchIndex.SearchInstanceId.HasValue)
            {
                throw new Exception($"{nameof(searchIndex.SearchInstanceId)} is null");
            }

            var SearchInstanceKeyGenerationRequest = ConvertToSearchInstanceQueryKeyDeletionRequest(keyDeletionRequest, searchIndex);

            var message = _queueManager.CreateMessage(SearchIndexQueues.DeleteKeys, SearchInstanceKeyGenerationRequest);
            await _queueManager.EnqueueMessageAsync(message);
        }

        public async Task<SearchIndex> GetByFriendlyNameAsync(Guid customerId, string friendlyName)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId },
                { "FriendlyName", friendlyName }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndex>(_connectionString,
                                                                                 StoredProcedures.GetSearchIndexByFriendlyName,
                                                                                 parameters);

            return result;
        }

        public async Task<SearchIndexQueryCredentials> GetQueryCredentials(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexQueryCredentials>(_connectionString,
                                                                                 StoredProcedures.GetSearchIndexQueryCredentials,
                                                                                 parameters);

            return result;
        }
    }
}
