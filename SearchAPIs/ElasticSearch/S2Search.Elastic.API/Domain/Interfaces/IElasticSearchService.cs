using Domain.Models.Request;
using Domain.Models.Response.Generic;
using Nest;

namespace Domain.Interfaces
{
    public interface IElasticSearchService
    {
        Task<ClusterHealthResponse> HealthCheck();
        Task<PingResponse> PingCheck();
        Task<SearchProductResult> InvokeSearch(SearchDataRequest request);
        Task<int> TotalDocumentCount(string index);
        Task<List<string>> AutoCompleteWithSuggestions(string searchTerm, string index);
    }
}