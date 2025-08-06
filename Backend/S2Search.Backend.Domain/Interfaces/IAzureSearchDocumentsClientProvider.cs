using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.AzureSearch
{
    public interface IAzureSearchDocumentsClientProvider
    {
        SearchIndexClient GetIndexClient(string searchServiceName, string endpoint, string apiKey);
        SearchClient GetSearchClient(string endpoint, string indexName, string apiKey);
    }
}