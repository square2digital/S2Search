using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using LazyCache;

namespace S2Search.Backend.Services.Services.Admin.Providers.AzureSearch
{
    public class AzureSearchClientProvider : IAzureSearchClientProvider
    {
        private readonly IAppCache _clientCache;

        public AzureSearchClientProvider(IAppCache clientCache)
        {
            _clientCache = clientCache;
        }

        public SearchClient GetSearchClient(string searchServiceName, string indexName, string apiKey)
        {
            var endpoint = GetEndpoint(searchServiceName);
            var credential = new AzureKeyCredential(apiKey);

            // Optionally cache by endpoint+indexName+apiKey
            var cacheKey = $"{endpoint}-{indexName}-{apiKey}";
            return _clientCache.GetOrAdd(cacheKey, () => new SearchClient(endpoint, indexName, credential));
        }

        public SearchIndexClient GetSearchIndexClient(string searchServiceName, string apiKey)
        {
            var endpoint = GetEndpoint(searchServiceName);
            var credential = new AzureKeyCredential(apiKey);

            // Optionally cache by endpoint+apiKey
            var cacheKey = $"{endpoint}-indexclient-{apiKey}";
            return _clientCache.GetOrAdd(cacheKey, () => new SearchIndexClient(endpoint, credential));
        }

        private static Uri GetEndpoint(string searchServiceName)
        {
            // Azure Search endpoint format: https://<service-name>.search.windows.net
            return new Uri($"https://{searchServiceName}.search.windows.net");
        }
    }
}