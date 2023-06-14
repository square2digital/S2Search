using Domain.Constants;
using Services.Customer.Interfaces.Repositories;
using Services.Dapper.Interfaces.Providers;
using Services.Customer.Interfaces.Managers;
using Domain.Customer.Constants;
using Domain.Customer.SearchResources.SearchIndex;
using Domain.Customer.SearchResources.CustomerPricing;
using Domain.Customer.SearchResources.SearchInstanceKeys;
using Domain.Customer.Enums;
using Domain.Customer.Shared;

namespace Services.Customer.Repositories
{
    public class SearchIndexRepository : ISearchIndexRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly IQueueManager _queueManager;

        public SearchIndexRepository(IDbContextProvider dbContext, IQueueManager queueManager)
        {
            _dbContext = dbContext;
            _queueManager = queueManager;
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
                var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndex>(ConnectionStrings.CustomerResourceStore,
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
                var result = await _dbContext.QueryMultipleAsync<SearchIndexFull>(ConnectionStrings.CustomerResourceStore,
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

            var result = await _dbContext.QueryAsync<SearchIndexKeys>(ConnectionStrings.CustomerResourceStore,
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

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndex>(ConnectionStrings.CustomerResourceStore,
                                                                                 StoredProcedures.GetSearchIndexByFriendlyName,
                                                                                 parameters);

            return result;
        }

        public async Task<IEnumerable<CustomerPricingTier>> GetPricingTiers()
        {
            var parameters = new Dictionary<string, object>();

            var result = await _dbContext.QueryAsync<CustomerPricingTier>(ConnectionStrings.CustomerResourceStore,
                                                                                 StoredProcedures.GetActiveCustomerPricingTiers,
                                                                                 parameters);

            return result;
        }

        public async Task<SearchIndexQueryCredentials> GetQueryCredentials(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexQueryCredentials>(ConnectionStrings.CustomerResourceStore,
                                                                                 StoredProcedures.GetSearchIndexQueryCredentials,
                                                                                 parameters);

            return result;
        }
    }
}
