using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models.Objects;
using Domain.Models.Facets;
using Domain.Models.Request;
using Domain.Models.Response;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Domain.Models.Interfaces;
using Services.Providers.AzureSearch;
using Azure.Search.Documents;
using Services.Interfaces.FacetOverrides;
using Domain.Constants;
using Services.Interfaces.Cache;
using Domain.Configuration.SearchResources.Credentials;

namespace Services.Services
{
    public class AzureFacetService : AzureSearchBase, IAzureFacetService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IAzureSearchDocumentsClientProvider _searchClientProvider;
        private readonly ISearchOptionsProvider _searchOptionsProvider;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ISearchIndexQueryCredentialsProvider _queryCredentialsProvider;

        public AzureFacetService(IAppSettings appSettings,
                                ILoggerFactory loggerFactory,
                                ISearchOptionsProvider searchCriteriaProvider,
                                IDisplayTextFormatHelper displayTextFormatHelper,
                                IAzureSearchDocumentsClientProvider searchClientProvider,
                                IFacetHelper facetHelper,
                                IFacetOverrideProvider facetOverrideProvider,
                                IMemoryCacheService memoryCacheService,
                                ISearchIndexQueryCredentialsProvider queryCredentialsProvider)

            : base(appSettings, loggerFactory, displayTextFormatHelper, facetHelper, facetOverrideProvider)
        {
            _appSettings = appSettings;
            _logger = loggerFactory.CreateLogger<AzureSearchService>();
            _searchClientProvider = searchClientProvider;
            _searchOptionsProvider = searchCriteriaProvider;
            _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
            _queryCredentialsProvider = queryCredentialsProvider ?? throw new ArgumentNullException(nameof(queryCredentialsProvider));
        }

        /// <summary>
        /// The default facets are all the facets returned from a search with no filters or search term
        /// these are used on first load and for generating lucence search strings 
        /// </summary>
        /// <param name="callingHost"></param>
        /// <param name="targetSearchResource"></param>
        /// <returns></returns>
        public async Task<IList<FacetGroup>> GetDefaultFacets(string callingHost, SearchIndexQueryCredentials targetSearchResource)
        {
            SearchRequest request = new SearchRequest
            {
                SearchTerm = "",
                Filters = "",
                OrderBy = null,
                PageNumber = 0,
                PageSize = 0,
                NumberOfExistingResults = 0,
                CallingHost = callingHost,
            };

            var data = await InvokeFacetRequest(request, targetSearchResource);
            return data.Facets;
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
        public IList<FacetGroup> GetOrSetDefaultFacets(string callingHost, SearchIndexQueryCredentials queryCredentials)
        {
            var cacheKey = $"{CacheKeys.DefaultFacets}";
            var func = GetDefaultFacetsFunc(callingHost, queryCredentials);
            return _memoryCacheService.GetOrAdd(cacheKey, func, TimeSpan.FromSeconds(_appSettings.MemoryCacheSettings.DefaultFacetsCacheExpiryInSeconds));
        }

        private Func<IList<FacetGroup>> GetDefaultFacetsFunc(string callingHost, SearchIndexQueryCredentials queryCredentials)
        {
            return () =>
            {
                _logger.LogInformation($"Cache miss on Default Facets | callingHost: {callingHost}");
                return GetDefaultFacets(callingHost, queryCredentials).GetAwaiter().GetResult();
            };
        }

        private async Task<SearchProductResult> InvokeFacetRequest(SearchRequest request, SearchIndexQueryCredentials targetSearchResource)
        {
            try
            {
                var searchParams = _searchOptionsProvider.CreateSearchOptions(request);
                var result = await GetSearchClient(targetSearchResource).SearchAsync<SearchVehicle>("", searchParams).ConfigureAwait(false);
                var facetGroups = result.Value.Facets;
                var totalResults = result.Value.TotalCount;

                var searchResult = new SearchProductResult()
                {
                    Facets = base.ConvertFacetsToResult(facetGroups),
                    Results = new List<SearchVehicle>(),
                    TotalResults = Convert.ToInt32(totalResults),
                };

                return searchResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(InvokeFacetRequest)} | Message: {ex.Message}");
                throw;
            }
        }

        private SearchClient GetSearchClient(SearchIndexQueryCredentials targetSearchResource)
        {
            return _searchClientProvider.GetSearchClient(targetSearchResource.SearchInstanceEndpoint,
                targetSearchResource.SearchIndexName,
                targetSearchResource.ApiKey);
        }
    }
}
