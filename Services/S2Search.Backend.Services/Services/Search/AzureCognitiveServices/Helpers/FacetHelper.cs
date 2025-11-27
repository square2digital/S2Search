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
        private readonly IAzureSearchService _azureSearchService;

        private IList<FacetGroup> _defaultFacets;

        public FacetHelper(IAzureSearchService azureSearchService,
            IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _azureSearchService = azureSearchService ?? throw new ArgumentNullException(nameof(azureSearchService));
        }

        public async Task<IList<FacetGroup>> GetDefaultFacets(string customerEndpoint, SearchIndexQueryCredentials queryCredentials)
        {
            if (_defaultFacets == null)
            {
                var request = new SearchRequest
                {
                    SearchTerm = "",
                    Filters = "",
                    OrderBy = null,
                    PageNumber = 0,
                    PageSize = 0,
                    NumberOfExistingResults = 0,
                    CustomerEndpoint = customerEndpoint,
                };

                var rats = await _azureSearchService.InvokeSearchRequest(request, queryCredentials);

                _defaultFacets = rats.SearchProductResult.Facets;
            }

            return _defaultFacets;
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
