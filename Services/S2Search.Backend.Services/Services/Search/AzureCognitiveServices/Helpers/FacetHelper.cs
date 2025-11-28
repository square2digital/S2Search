using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers
{
    public class FacetHelper : IFacetHelper
    {
        private readonly IAppSettings _appSettings;

        public FacetHelper(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public IList<FacetGroup> SetFacetOrder(IList<FacetGroup> facets)
        {
            return OrderFacets(facets);
        }

        private IList<FacetGroup> OrderFacets(IList<FacetGroup> facets)
        {
            var facetList = new List<FacetGroup>();

            foreach (var facetName in _appSettings.SearchSettings.FacetOrderList)
            {
                FacetGroup facet = facets.Where(x => x.FacetKey == facetName).SingleOrDefault();
                if (facet != null)
                {
                    facet.FacetItems = facet.FacetItems.OrderBy(x => x.Value).ToList();
                    facetList.Add(facet);
                }
            }

            return facetList;
        }
    }
}
