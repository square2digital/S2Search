using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace Services.Providers.AzureSearch
{
    public interface IAzureSearchDocumentsClientProvider
    {
        SearchIndexClient GetIndexClient(string searchServiceName, string endpoint, string apiKey);
        SearchClient GetSearchClient(string endpoint, string indexName, string apiKey);
    }
}