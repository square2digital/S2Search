using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

public interface IAzureFacetService
{
    Task<IList<FacetGroup>> GetDefaultFacets(string customerEndpoint, SearchIndexQueryCredentials queryCredentials);
    IList<FacetGroup> GetOrSetDefaultFacets(string customerEndpoint, SearchIndexQueryCredentials queryCredentials);
}