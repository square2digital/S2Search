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
using Domain.AzureSeach.Indexes;
using Domain.Constants;
using Services.Services;
using Newtonsoft.Json;
using S2SearchAPI.Client;
using Domain.Models.Insights;

namespace Services
{
    public class AzureSearchService : AzureSearchBase, IAzureSearchService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IAzureSearchDocumentsClientProvider _searchClientProvider;
        private readonly IDisplayTextFormatHelper _displayTextFormatHelper;
        private readonly ISearchOptionsProvider _searchOptionsProvider;
        private readonly ILuceneSyntaxHelper _luceneSyntaxHelper;
        private readonly IFacetHelper _facetHelper;
        private readonly IAzureAutoSuggestOptionsProvider _autoSuggestOptionsProvider;

        public AzureSearchService(IAppSettings appSettings,
                                  ILoggerFactory loggerFactory,
                                  ISearchOptionsProvider searchCriteriaProvider,
                                  IDisplayTextFormatHelper displayTextFormatHelper,
                                  IAzureSearchDocumentsClientProvider searchClientProvider,
                                  ILuceneSyntaxHelper luceneSyntaxHelper,
                                  IFacetHelper facetHelper,
                                  IFacetOverrideProvider facetOverrideProvider,
                                  IAzureAutoSuggestOptionsProvider autoSuggestOptionsProvider) : base(appSettings,
                                                                                                      loggerFactory,
                                                                                                      displayTextFormatHelper,
                                                                                                      facetHelper,
                                                                                                      facetOverrideProvider)
        {
            _logger = loggerFactory.CreateLogger<AzureSearchService>();
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _searchClientProvider = searchClientProvider ?? throw new ArgumentNullException(nameof(searchClientProvider));
            _displayTextFormatHelper = displayTextFormatHelper ?? throw new ArgumentNullException(nameof(displayTextFormatHelper));
            _searchOptionsProvider = searchCriteriaProvider ?? throw new ArgumentNullException(nameof(searchCriteriaProvider));
            _luceneSyntaxHelper = luceneSyntaxHelper ?? throw new ArgumentNullException(nameof(luceneSyntaxHelper));
            _facetHelper = facetHelper ?? throw new ArgumentNullException(nameof(facetHelper));
            _autoSuggestOptionsProvider = autoSuggestOptionsProvider ?? throw new ArgumentNullException(nameof(autoSuggestOptionsProvider));
        }

        public async Task<SearchResultRoot> InvokeSearchRequest(SearchRequest request, SearchIndexQueryCredentials targetSearchResource)
        {
            ValidateRequest(request, targetSearchResource);

            SearchOptions searchOptions = new SearchOptions();
            string luceneSearch = string.Empty;

            try
            {
                if (_luceneSyntaxHelper.ContainsSpecialCharacters(request.SearchTerm))
                {
                    request.SearchTerm = _luceneSyntaxHelper.EscapeLuceneSpecialCharacters(request.SearchTerm);
                }

                searchOptions = _searchOptionsProvider.CreateSearchOptions(request);
                luceneSearch = _luceneSyntaxHelper.GenerateLuceneSearchString(request.SearchTerm, request.CallingHost);

                var result = await GetSearchClient(targetSearchResource).SearchAsync<SearchVehicle>(string.Join(" ", luceneSearch), searchOptions).ConfigureAwait(false);
                var facetGroups = result.Value.Facets;
                var totalResults = result.Value.TotalCount;

                IList<SearchVehicle> searchVehicleResults = new List<SearchVehicle>();

                foreach (var item in result.Value.GetResults())
                {
                    searchVehicleResults.Add(item.Document);
                }

                var searchInsightMessage = CreateSearchInsightMessage(request, targetSearchResource, luceneSearch, totalResults);

                var searchResult = new SearchResultRoot()
                {
                    SearchProductResult = new SearchProductResult()
                    {
                        Facets = ConvertFacetsToResult(facetGroups),
                        Results = searchVehicleResults,
                        TotalResults = Convert.ToInt32(totalResults ?? 0),
                    },
                    SearchInsightMessage = searchInsightMessage
                };

                return searchResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(InvokeSearchRequest)} | Message: {ex.Message}");
                _logger.LogInformation($"Lucene Search {luceneSearch}");
                _logger.LogInformation($"Request {JsonConvert.SerializeObject(request)}");
                _logger.LogInformation($"Search Options {JsonConvert.SerializeObject(searchOptions)}");
                throw;
            }
        }

        private static SearchInsightMessage CreateSearchInsightMessage(SearchRequest request, SearchIndexQueryCredentials targetSearchResource, string luceneSearch, long? totalResults)
        {
            return new SearchInsightMessage()
            {
                SearchIndexId = targetSearchResource.SearchIndexId,
                ActualSearchQuery = request.SearchTerm,
                LuceneSearchQuery = luceneSearch,
                Filters = request.Filters,
                OrderBy = request.OrderBy,
                TotalResults = Convert.ToInt32(totalResults ?? 0)
            };
        }

        public async Task<int> TotalDocumentCount(SearchIndexQueryCredentials targetSearchResource)
        {
            if (targetSearchResource == null)
            {
                throw new ArgumentNullException(nameof(targetSearchResource));
            }

            var result = await GetSearchClient(targetSearchResource).GetDocumentCountAsync().ConfigureAwait(false);

            return Convert.ToInt32(result);
        }

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

            var data = await InvokeSearchRequest(request, targetSearchResource);
            return data.SearchProductResult.Facets;
        }

        private void ValidateRequest(SearchRequest request, SearchIndexQueryCredentials targetSearchResource)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (targetSearchResource == null) throw new ArgumentNullException(nameof(targetSearchResource));
        }

        private SearchClient GetSearchClient(SearchIndexQueryCredentials targetSearchResource)
        {
            return _searchClientProvider.GetSearchClient(targetSearchResource.SearchInstanceEndpoint, targetSearchResource.SearchIndexName, targetSearchResource.QueryApiKey);
        }

        public async Task<IEnumerable<string>> AutocompleteWithSuggestions(string searchTerm, SearchIndexQueryCredentials targetSearchResource)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            try
            {
                var searchClient = GetSearchClient(targetSearchResource);
                var autoSuggestOptions = _autoSuggestOptionsProvider.Get(AzureAutoSuggest.DefaultSuggesterName,
                                                                         AzureAutoSuggest.DefaultSuggestSelectFields,
                                                                         AzureAutoSuggest.DefaultSuggestSearchFields,
                                                                         AzureAutoSuggest.DefaultAutocompleteSearchFields);

                var autocompleteResult = await searchClient.AutocompleteAsync(searchTerm, autoSuggestOptions.SuggesterName, autoSuggestOptions.AutocompleteOptions);
                var suggestionResults = await searchClient.SuggestAsync<VehicleIndex>(searchTerm, autoSuggestOptions.SuggesterName, autoSuggestOptions.SuggestOptions);

                var suggestions = new List<string>();

                var autocompleteText = "";
                if (autocompleteResult.Value.Results.Count > 0)
                {
                    autocompleteText = autocompleteResult.Value.Results[0].Text;
                }

                //Add the autocomplete suggestion first
                suggestions.Add(autocompleteText);

                foreach (var suggestion in suggestionResults.Value.Results)
                {
                    var newSuggestion = $"{suggestion.Document.Make} {suggestion.Document.Model}";

                    if (suggestions.Contains(newSuggestion))
                    {
                        continue;
                    }

                    suggestions.Add(newSuggestion);
                }

                return suggestions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(AutocompleteWithSuggestions)} | Message: {ex.Message}");
                throw;
            }
        }
    }
}