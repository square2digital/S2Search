using Microsoft.Azure.Search;

namespace S2Search.Backend.Services.Services.Admin.Providers.AzureSearch
{
    public interface IAzureSearchClientProvider
    {
        ISearchServiceClient GetServiceClient(string searchServiceName, string apiKey);
        ISearchIndexClient GetIndexClient(string searchServiceName, string indexName, string apiKey);
    }
}
