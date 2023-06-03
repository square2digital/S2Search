using Domain.Models.Request;
using Domain.Models.Response.Generic;

namespace Services.Interfaces
{
    public interface IElasticFacetService
    {
        Task<SearchProductResult> GetDefaultFacets(string index);
        Task<SearchProductResult> GetSearchFacets(SearchDataRequest request);
        SearchProductResult GetOrSetDefaultFacets(string index);
    }
}