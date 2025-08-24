using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace S2.Importer.Providers.AzureSearch
{
    public interface IAzureSearchDocumentsClientProvider
    {
        SearchIndexClient GetIndexClient();
        SearchClient GetSearchClient();
        SearchIndexerClient GetSearchIndexerClient();
    }
}