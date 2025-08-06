using S2Search.Backend.Domain.Models.Response;

namespace S2Search.Backend.Domain.Interfaces
{
    public interface IElasticFacetService
    {
        Task<SearchProductResult> GetDefaultFacets(string index);
        //Task<SearchProductResult> GetSearchFacets(SearchDataRequest request);
        SearchProductResult GetOrSetDefaultFacets(string index);
    }
}