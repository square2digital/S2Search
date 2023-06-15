using LazyCache;
using Microsoft.Azure.Search;

namespace S2Search.Common.Providers.AzureSearch
{
    public class AzureSearchClientProvider : IAzureSearchClientProvider
    {
        private readonly IAppCache _clientCache;

        public AzureSearchClientProvider(IAppCache clientCache)
        {
            _clientCache = clientCache;
        }

        public ISearchIndexClient GetIndexClient(string searchServiceName, string indexName, string apiKey)
        {
            var client = GetServiceClient(searchServiceName, apiKey);
            var indexClient = client.Indexes.GetClient(indexName);

            return indexClient;
        }

        public ISearchServiceClient GetServiceClient(string searchServiceName, string apiKey)
        {
            return _clientCache.GetOrAdd(searchServiceName, () => CreateServiceClient(searchServiceName, apiKey));
        }

        private static ISearchServiceClient CreateServiceClient(string searchServiceName, string apiKey)
        {
            return new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
        }
    }
}
