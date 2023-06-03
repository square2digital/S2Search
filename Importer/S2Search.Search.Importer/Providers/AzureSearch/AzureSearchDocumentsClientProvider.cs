using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using LazyCache;
using Microsoft.Extensions.Options;
using S2.Test.Importer;
using System;

namespace S2.Importer.Providers.AzureSearch
{
    public class AzureSearchDocumentsClientProvider : IAzureSearchDocumentsClientProvider
    {
        private readonly AppSettings _AppSettings;
        private readonly IAppCache _clientCache;

        public AzureSearchDocumentsClientProvider(IAppCache clientCache,
            IOptionsSnapshot<AppSettings> appSettings)
        {
            _clientCache = clientCache;
            _AppSettings = appSettings.Value;
        }

        public SearchClient GetSearchClient()
        {
            return _clientCache.GetOrAdd(_AppSettings.IndexSettings.SearchIndexName, () =>
                {
                    return new SearchClient(new Uri(_AppSettings.SearchSettings.SearchServiceEndpoint), _AppSettings.IndexSettings.SearchIndexName, GetAzureKeyCredential(_AppSettings.APIKeys.PrimaryAdminKey));
                }
            );
        }

        public SearchIndexClient GetIndexClient()
        {
            return _clientCache.GetOrAdd(_AppSettings.IndexSettings.SearchIndexName, () =>
                {
                    return new SearchIndexClient(new Uri(_AppSettings.SearchSettings.SearchServiceEndpoint), GetAzureKeyCredential(_AppSettings.APIKeys.PrimaryAdminKey));
                }
            );
        }

        public SearchIndexerClient GetSearchIndexerClient()
        {
            return _clientCache.GetOrAdd(_AppSettings.IndexSettings.SearchIndexName, () =>
                {
                    return new SearchIndexerClient(new Uri(_AppSettings.SearchSettings.SearchServiceEndpoint), GetAzureKeyCredential(_AppSettings.APIKeys.PrimaryAdminKey));
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
