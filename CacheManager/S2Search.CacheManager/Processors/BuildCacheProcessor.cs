using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services.Cache;
using S2Search.CacheManager.Interfaces.Processors;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Search.CacheManager.Processors
{
    internal class BuildCacheProcessor : IBuildCacheProcessor
    {
        private readonly IAzureFacetService _azureFacetService;
        private readonly IDistributedCacheService _redisService;
        private readonly ISearchIndexQueryCredentialsProvider _queryCredentialsProvider;

        public BuildCacheProcessor(IAzureFacetService azureFacetService,
            IDistributedCacheService redisService,
            ISearchIndexQueryCredentialsProvider queryCredentialsProvider)
        {
            _azureFacetService = azureFacetService ?? throw new ArgumentNullException(nameof(azureFacetService));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _queryCredentialsProvider = queryCredentialsProvider ?? throw new ArgumentNullException(nameof(queryCredentialsProvider));
        }

        public async Task ProcessAsync(string customerEndpoint)
        {
            var searchRequest = GetDefaultSearchRequest(customerEndpoint);
            var queryCredentials = await _queryCredentialsProvider.GetAsync(customerEndpoint);
            var facetResults = _azureFacetService.GetOrSetDefaultFacets(customerEndpoint, queryCredentials);
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

