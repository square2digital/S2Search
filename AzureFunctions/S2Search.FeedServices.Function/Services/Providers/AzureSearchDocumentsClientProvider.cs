using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using LazyCache;
using Services.Interfaces.Providers;
using System;

namespace Services.Providers
{
    public class AzureSearchDocumentsClientProvider : IAzureSearchDocumentsClientProvider
    {
        private readonly IAppCache _clientCache;

        public AzureSearchDocumentsClientProvider(IAppCache clientCache)
        {
            _clientCache = clientCache;
        }

        public SearchClient GetSearchClient(string searchServiceEndpoint, string searchIndexName, string searchKey)
        {
            return _clientCache.GetOrAdd(searchIndexName, () =>
                {
                    return new SearchClient(new Uri(searchServiceEndpoint), searchIndexName, GetAzureKeyCredential(searchKey));
                }
            );
        }

        public SearchIndexClient GetIndexClient(string searchServiceEndpoint, string searchIndexName, string searchKey)
        {
            return _clientCache.GetOrAdd(searchIndexName, () =>
                {
                    return new SearchIndexClient(new Uri(searchServiceEndpoint), GetAzureKeyCredential(searchKey));
                }
            );
        }

        public SearchIndexerClient GetSearchIndexerClient(string searchServiceEndpoint, string searchIndexName, string searchKey)
        {
            return _clientCache.GetOrAdd(searchIndexName, () =>
                {
                    return new SearchIndexerClient(new Uri(searchServiceEndpoint), GetAzureKeyCredential(searchKey));
                }
            );
        }

        private AzureKeyCredential GetAzureKeyCredential(string apiKey)
        {
            AzureKeyCredential azureKeyCredential = new AzureKeyCredential(apiKey);
            return azureKeyCredential;
        }
    }
}
