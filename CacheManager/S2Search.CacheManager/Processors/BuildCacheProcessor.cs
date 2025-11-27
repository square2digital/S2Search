using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.CacheManager.Interfaces.Processors;
using Services.Interfaces;

namespace S2Search.CacheManager.Processors
{
    internal sealed class BuildCacheProcessor : IBuildCacheProcessor
    {
        private readonly IAzureSearchService _azureSearchService;
        private readonly IAzureFacetService _azureFacetService;
        private readonly IDistributedCacheService _redisService;
        private readonly ISearchIndexQueryCredentialsProvider _queryCredentialsProvider;
        private readonly IAppSettings _appSettings;

        private static readonly IReadOnlyList<string> DefaultFacetKeys = new List<string> { "Make", "Model", "Location", "Year", "Transmission", "FuelType", "BodyStyle", "Doors", "Colour" };

        private IList<FacetGroup> _facetItems = new List<FacetGroup>();

        public BuildCacheProcessor(
            IAzureSearchService azureSearchService,
            IAzureFacetService azureFacetService,
            IDistributedCacheService redisService,
            ISearchIndexQueryCredentialsProvider queryCredentialsProvider,
            IAppSettings appSettings)
        {
            _azureSearchService = azureSearchService ?? throw new ArgumentNullException(nameof(azureSearchService));
            _azureFacetService = azureFacetService ?? throw new ArgumentNullException(nameof(azureFacetService));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _queryCredentialsProvider = queryCredentialsProvider ?? throw new ArgumentNullException(nameof(queryCredentialsProvider));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task ProcessAsync(string customerEndpoint)
        {
            var queryCredentials = await _queryCredentialsProvider.GetAsync(customerEndpoint).ConfigureAwait(false);
            _facetItems = _azureFacetService.GetOrSetDefaultFacets(customerEndpoint, queryCredentials);

            var tasks = new List<Task>
            {
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Model"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Location"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Year"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Transmission"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Colour"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "FuelType"),

                BuildRequestsForFacetsAsync(customerEndpoint, "Model", "Location"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Model", "Year"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Model", "Transmission"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Model", "Colour"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Model", "FuelType"),

                BuildRequestsForFacetsAsync(customerEndpoint, "Location", "Transmission"),
                BuildRequestsForFacetsAsync(customerEndpoint, "FuelType", "Year"),
                BuildRequestsForFacetsAsync(customerEndpoint, "BodyStyle", "Year"),
                BuildRequestsForFacetsAsync(customerEndpoint, "BodyStyle", "Transmission"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Colour", "BodyStyle"),
                BuildRequestsForFacetsAsync(customerEndpoint, "BodyStyle", "Colour"),

                BuildRequestsForFacetsAsync(customerEndpoint, "Make"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Model"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Location"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Year"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Transmission"),
                BuildRequestsForFacetsAsync(customerEndpoint, "FuelType"),
                BuildRequestsForFacetsAsync(customerEndpoint, "BodyStyle"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Colour"),

                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Model", "Location"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Model", "FuelType"),
                BuildRequestsForFacetsAsync(customerEndpoint, "Make", "Model", "Location", "Colour"),
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task BuildRequestsForFacetsAsync(string customerEndpoint, params string[] facetKeys)
        {
            if (facetKeys == null || facetKeys.Length == 0)
            {
                return;
            }

            // build a lookup of requested facet groups
            var facetLookup = _facetItems
                .Where(fg => facetKeys.Contains(fg.FacetName, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(fg => fg.FacetName, fg => fg.FacetItems);

            if (facetLookup.Count == 0)
            {
                return;
            }

            // reuse credentials across loop iterations
            var creds = await _queryCredentialsProvider.GetAsync(customerEndpoint).ConfigureAwait(false);

            foreach (var facetList in facetLookup.Values)
            {
                if (facetList == null)
                {
                    continue;
                }

                foreach (var facetItem in facetList)
                {
                    if (facetItem == null || string.IsNullOrWhiteSpace(facetItem.Value))
                    {
                        continue;
                    }

                    var request = BuildFacetSearchRequest(customerEndpoint, facetItem.Value);
                    var keyPayload = JsonConvert.SerializeObject(request);
                    var redisKey = _redisService.CreateRedisKey(request.CustomerEndpoint, "search", HashHelper.GetXXHashString(keyPayload));

                    if (_appSettings.RedisCacheSettings.EnableRedisCache && redisKey != null)
                    {
                        var existing = await _redisService.GetFromRedisIfExistsAsync(redisKey).ConfigureAwait(false);
                        if (!string.IsNullOrEmpty(existing))
                        {
                            continue;
                        }
                    }

                    var result = await _azureSearchService.InvokeSearchRequest(request, creds).ConfigureAwait(false);

                    if (_appSettings.RedisCacheSettings.EnableRedisCache && redisKey != null)
                    {
                        var serialized = JsonConvert.SerializeObject(result);
                        await _redisService.SetValueAsync(redisKey, serialized, CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds)).ConfigureAwait(false);
                    }
                }
            }
        }

        private static SearchRequest BuildFacetSearchRequest(string customerEndpoint, string searchTerm)
        {
            return new SearchRequest
            {
                PageNumber = 1,
                PageSize = 0,
                CustomerEndpoint = customerEndpoint,
                SearchTerm = searchTerm
            };
        }
    }
}

