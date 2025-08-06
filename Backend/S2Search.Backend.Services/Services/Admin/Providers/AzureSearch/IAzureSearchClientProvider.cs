using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace S2Search.Backend.Services.Services.Admin.Providers.AzureSearch
{
    public interface IAzureSearchClientProvider
    {
        SearchIndexClient GetSearchIndexClient(string searchServiceName, string apiKey);
        SearchClient GetSearchClient(string searchServiceName, string indexName, string apiKey);
    }
}
