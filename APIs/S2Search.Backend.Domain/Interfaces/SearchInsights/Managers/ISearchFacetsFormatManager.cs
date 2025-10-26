using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;

namespace S2Search.Backend.Domain.Interfaces.SearchInsights.Managers
{
    public interface ISearchFacetsFormatManager
    {
        public IEnumerable<SearchFacet> GetSearchFacets(string unformattedFacets);
    }
}
