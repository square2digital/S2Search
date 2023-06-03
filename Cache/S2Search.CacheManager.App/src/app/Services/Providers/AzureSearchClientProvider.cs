using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers
{
    internal class AzureSearchClientProvider
    {
        private readonly IAppSettings _appSettings;

        public AzureSearchDocumentsClientProvider(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public SearchClient GetSearchClient(string endpoint, string indexName, string apiKey)
        {
            var searchClientFactory = GetSearchClientFactory(endpoint, indexName, apiKey);
        }

        public SearchIndexClient GetIndexClient(string searchServiceName, string endpoint, string apiKey)
        {
            return GetSearchIndexClientFactory(endpoint, apiKey);
        }

        private SearchClient GetSearchClientFactory(string endpoint, string indexName, string apiKey)
        {
            return new SearchClient(new Uri(endpoint), indexName, GetAzureKeyCredential(apiKey));
        }

        private SearchIndexClient GetSearchIndexClientFactory(string endpoint, string apiKey)
        {
            return new SearchIndexClient(new Uri(endpoint), GetAzureKeyCredential(apiKey));
        }

        private AzureKeyCredential GetAzureKeyCredential(string apiKey)
        {
            AzureKeyCredential azureKeyCredential = new AzureKeyCredential(apiKey);
            return azureKeyCredential;
        }
    }
}
