using Elastic.Clients.Elasticsearch;
using S2Search.Backend.Domain.Models.Response;

namespace S2Search.Backend.Domain.Interfaces
{
    public interface IElasticSearchService
    {
        //Task<ClusterHealthResponse> HealthCheck();
        Task<PingResponse> PingCheck();
        //Task<SearchProductResult> InvokeSearch(SearchDataRequest request);
        Task<int> TotalDocumentCount(string index);
        Task<List<string>> AutoCompleteWithSuggestions(string searchTerm, string index);
    }
}