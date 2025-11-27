using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services;
using S2Search.CacheManager.Interfaces.Processors;
using Services.Interfaces;
using System.Collections.Generic;

namespace S2Search.CacheManager.Processors
{
    internal class BuildCacheProcessor : IBuildCacheProcessor
    {
        private readonly IAzureSearchService _azureSearchService;
        private readonly IAzureFacetService _azureFacetService;
        private readonly IDistributedCacheService _redisService;
        private readonly ISearchIndexQueryCredentialsProvider _queryCredentialsProvider;
        private readonly List<string> _facetKeys;

        private IList<FacetGroup> _facetItems = new List<FacetGroup>();

        public BuildCacheProcessor(IAzureSearchService azureSearchService,
            IAzureFacetService azureFacetService,
            IDistributedCacheService redisService,
            ISearchIndexQueryCredentialsProvider queryCredentialsProvider)
        {
            _azureSearchService = azureSearchService ?? throw new ArgumentNullException(nameof(azureSearchService));
            _azureFacetService = azureFacetService ?? throw new ArgumentNullException(nameof(azureFacetService));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _queryCredentialsProvider = queryCredentialsProvider ?? throw new ArgumentNullException(nameof(queryCredentialsProvider));
            _facetKeys = new List<string>() { "Make", "Model", "Location", "Year", "Transmission", "FuelType", "BodyStyle", "Doors", "Colour" };
        }

        public async Task ProcessAsync(string customerEndpoint)
        {
            var searchRequest = GetDefaultSearchRequest(customerEndpoint);
            var queryCredentials = await _queryCredentialsProvider.GetAsync(customerEndpoint);
            _facetItems = _azureFacetService.GetOrSetDefaultFacets(customerEndpoint, queryCredentials);

            await BuildRequest(customerEndpoint, "Make", "Model");
        }

        private async Task BuildRequest(string customerEndpoint, params string[] facetKeys)
        {
            string searchTerm = "";

            Dictionary<string, IList<FacetItem>> facetItems = new Dictionary<string, IList<FacetItem>>();

            for (int i = 0; i < facetKeys.Length; i++)
            {
                var results = _facetItems.Where(x => x.FacetName == facetKeys[i]).SingleOrDefault();
                if (results != null)
                {
                    facetItems.Add(facetKeys[i], results.FacetItems);
                }
            }

            foreach (var facetKey in facetItems.Keys)
            {
                foreach (var facetItem in facetItems[facetKey])
                {
                    searchTerm = facetItem.Value;
                    await _azureSearchService.InvokeSearchRequest(
                        BuildFacetSearchRequest(customerEndpoint, searchTerm),
                        await _queryCredentialsProvider.GetAsync(customerEndpoint));
                }
            }
        }

        private SearchRequest GetDefaultSearchRequest(string customerEndpoint)
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

            return request;
        }

        private SearchRequest BuildFacetSearchRequest(string customerEndpoint, string searchTerm)
        {
            var request = new SearchRequest
            {
                PageNumber = 1,
                PageSize = 0,
                CustomerEndpoint = customerEndpoint,
                SearchTerm = searchTerm
            };

            return request;
        }
    }
}

