using Domain.Extentions;
using Domain.Interfaces;
using Domain.Models.Request;
using Domain.Models.Response.Generic;
using Microsoft.Extensions.Logging;
using Services.Helper;
using Services.Helpers;
using Services.Interfaces;
using Services.Interfaces.FacetOverrides;

namespace Services.Services
{
    public class ElasticFacetService : ElasticSearchBase, IElasticFacetService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IElasticSearchClientProvider _elasticSearchClientProvider;
        private readonly IElasticIndexService _elasticIndexService;
        private readonly IDisplayTextFormatHelper _displayTextFormatHelper;

        public ElasticFacetService(IAppSettings appSettings,
                                    ILoggerFactory loggerFactory,
                                    IElasticSearchClientProvider elasticSearchClientProvider,
                                    IDisplayTextFormatHelper displayTextFormatHelper,
                                    IFacetHelper facetHelper,
                                    IFacetOverrideProvider facetOverrideProvider,
                                    IElasticIndexService elasticIndexService,
                                    ILogger<ElasticSearchService> logger)
            
            : base(appSettings,
                  loggerFactory,
                  displayTextFormatHelper,
                  facetHelper,
                  facetOverrideProvider,
                  elasticIndexService,
                  elasticSearchClientProvider)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _elasticSearchClientProvider = elasticSearchClientProvider ?? throw new ArgumentNullException(nameof(elasticSearchClientProvider));
            _displayTextFormatHelper = displayTextFormatHelper ?? throw new ArgumentNullException(nameof(displayTextFormatHelper));
            _elasticIndexService = elasticIndexService ?? throw new ArgumentNullException(nameof(elasticIndexService));
            _logger = loggerFactory.CreateLogger<ElasticFacetService>();
        }

        /// <summary>
        /// The default facets are all the facets returned from a search with no filters or search term
        /// these are used on first load and for generating lucence search strings 
        /// </summary>
        /// <param name="callingHost"></param>
        /// <param name="targetSearchResource"></param>
        /// <returns></returns>
        public async Task<SearchProductResult> GetDefaultFacets(string index)
        {
            var request = new SearchDataRequest();
            request.Index = index;
            var data = await base.InvokeSearchRequest(request);
            return data;
        }

        /// <summary>
        /// This will return the facets based on a search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchProductResult> GetSearchFacets(SearchDataRequest request)
        {
            var data = await base.InvokeSearchRequest(request);
            return data;
        }

        /// <summary>
        /// This will add the default facets to the cache by customer using the callingHost for the cache key
        /// its intended to be called via "fire and forget" rather than async awaiting
        /// Once we have the FTP solution setup we can generate the default factes for a customer at the point of updating an indxex
        /// and store that in a distributed cache store such as redis -
        /// 
        /// for now this will save the defaut facets to memory cache on the call to the facets endpoint which occurs as soon as the search UI starts
        /// </summary>
        /// <param name="callingHost"></param>
        /// <returns></returns>
        public SearchProductResult GetOrSetDefaultFacets(string index)
        {
            _logger.LogInformation($"Cache miss on Default Facets");
            return GetDefaultFacets(index).Result;
        }
    }
}