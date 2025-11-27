using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Domain.Interfaces;

public interface IFacetHelper
{
    Task<IList<FacetGroup>> GetDefaultFacets(string customerEndpoint, SearchIndexQueryCredentials queryCredentials);
    IList<FacetGroup> SetFacetOrder(IList<FacetGroup> Facets);
}