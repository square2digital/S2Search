using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using System;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.AzureSearch
{
    public class AzureSearchDocumentsClientProvider : IAzureSearchDocumentsClientProvider
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IAppSettings _appSettings;

        public AzureSearchDocumentsClientProvider(IMemoryCacheService memoryCacheService, IAppSettings appSettings)
        {
            _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public SearchClient GetSearchClient(string endpoint, string indexName, string apiKey)
        {
            var searchClientFactory = GetSearchClientFactory(endpoint, indexName, apiKey);
            return _memoryCacheService.GetOrAdd(indexName, searchClientFactory, TimeSpan.FromSeconds(_appSettings.MemoryCacheSettings.DefaultFacetsCacheExpiryInSeconds));
        }

        public SearchIndexClient GetIndexClient(string searchServiceName, string endpoint, string apiKey)
        {
            var searchIndexClientFactory = GetSearchIndexClientFactory(endpoint, apiKey);
            return _memoryCacheService.GetOrAdd(searchServiceName, searchIndexClientFactory, TimeSpan.FromSeconds(_appSettings.MemoryCacheSettings.DefaultFacetsCacheExpiryInSeconds));
        }

        private Func<SearchClient> GetSearchClientFactory(string endpoint, string indexName, string apiKey)
        {
            return () =>
            {
                return new SearchClient(new Uri(endpoint), indexName, GetAzureKeyCredential(apiKey));
            };
        }

        private Func<SearchIndexClient> GetSearchIndexClientFactory(string endpoint, string apiKey)
        {
            return () =>
            {
                return new SearchIndexClient(new Uri(endpoint), GetAzureKeyCredential(apiKey));
            };
        }

        private AzureKeyCredential GetAzureKeyCredential(string apiKey)
        {
            AzureKeyCredential azureKeyCredential = new AzureKeyCredential(apiKey);
            return azureKeyCredential;
        }
    }
}
