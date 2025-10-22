using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace Services.Interfaces.Providers
{
    public interface IAzureSearchDocumentsClientProvider
    {
        SearchIndexClient GetIndexClient(string searchServiceEndpoint, string searchIndexName, string searchKey);
        SearchClient GetSearchClient(string searchServiceEndpoint, string searchIndexName, string searchKey);
        SearchIndexerClient GetSearchIndexerClient(string searchServiceEndpoint, string searchIndexName, string searchKey);
    }
}