using Domain.Models.Facets;
using S2Search.ClientConfigurationApi.Client.AutoRest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAzureFacetService
    {
        Task<IList<FacetGroup>> GetDefaultFacets(string callingHost, SearchIndexQueryCredentials targetSearchResource);
        IList<FacetGroup> GetOrSetDefaultFacets(string callingHost, SearchIndexQueryCredentials queryCredentials);
    }
}