using Microsoft.Azure.Search;

namespace Services.Providers.AzureSearch
{
    public interface IAzureSearchClientProvider
    {
        ISearchServiceClient GetServiceClient(string searchServiceName, string apiKey);
        ISearchIndexClient GetIndexClient(string searchServiceName, string indexName, string apiKey);
    }
}
