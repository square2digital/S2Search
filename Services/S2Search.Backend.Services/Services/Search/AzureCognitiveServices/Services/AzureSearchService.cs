using Azure.Search.Documents;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.AzureSearch.Indexes;
using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Interfaces.FacetOverrides;
using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Objects;
using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Domain.Models.Response;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.AzureSearch;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services
{
    public class AzureSearchService : AzureSearchBase, IAzureSearchService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IAzureSearchDocumentsClientProvider _searchClientProvider;
        private readonly IDisplayTextFormatHelper _displayTextFormatHelper;
        private readonly ISearchOptionsProvider _searchOptionsProvider;
        private readonly IFacetHelper _facetHelper;
        private readonly IAzureAutoSuggestOptionsProvider _autoSuggestOptionsProvider;

        public AzureSearchService(IAppSettings appSettings,
                                  ILoggerFactory loggerFactory,
                                  ISearchOptionsProvider searchCriteriaProvider,
                                  IDisplayTextFormatHelper displayTextFormatHelper,
                                  IAzureSearchDocumentsClientProvider searchClientProvider,
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
            _facetHelper = facetHelper ?? throw new ArgumentNullException(nameof(facetHelper));
            _autoSuggestOptionsProvider = autoSuggestOptionsProvider ?? throw new ArgumentNullException(nameof(autoSuggestOptionsProvider));
        }

        public async Task<SearchResultRoot> InvokeSearchRequest(SearchRequest request, SearchIndexQueryCredentials queryCredentials)
        {
            ValidateRequest (request, queryCredentials);

            SearchOptions searchOptions = new SearchOptions();
            string luceneSearch = string.Empty;

            try
            {
                if (LuceneSyntaxHelper.ContainsSpecialCharacters(request.SearchTerm))
                {
                    request.SearchTerm = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(request.SearchTerm);
                }

                searchOptions = _searchOptionsProvider.CreateSearchOptions(request);
                luceneSearch = LuceneSyntaxHelper.GenerateLuceneSearchString(request.SearchTerm, await _facetHelper.GetDefaultFacets(request.CustomerEndpoint, queryCredentials));

                var result = await GetSearchClient(queryCredentials).SearchAsync<SearchVehicle>(string.Join(" ", luceneSearch), searchOptions).ConfigureAwait(false);
                var facetGroups = result.Value.Facets;
                var totalResults = result.Value.TotalCount;

                IList<SearchVehicle> searchVehicleResults = new List<SearchVehicle>();

                foreach (var item in result.Value.GetResults())
                {
                    searchVehicleResults.Add(item.Document);
                }

                var searchInsightMessage = CreateSearchInsightMessage(request, queryCredentials, luceneSearch, totalResults);

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

        private static SearchInsightMessage CreateSearchInsightMessage(SearchRequest request, SearchIndexQueryCredentials queryCredentials, string luceneSearch, long? totalResults)
        {
            return new SearchInsightMessage()
            {
                SearchIndexId = queryCredentials.id,
                ActualSearchQuery = request.SearchTerm,
                LuceneSearchQuery = luceneSearch,
                Filters = request.Filters,
                OrderBy = request.OrderBy,
                TotalResults = Convert.ToInt32(totalResults ?? 0)
            };
        }

        public async Task<int> TotalDocumentCount(SearchIndexQueryCredentials queryCredentials)
        {
            if (queryCredentials == null)
            {
                throw new ArgumentNullException(nameof(queryCredentials));
            }

            var result = await GetSearchClient(queryCredentials).GetDocumentCountAsync().ConfigureAwait(false);

            return Convert.ToInt32(result);
        }

        public async Task<IList<FacetGroup>> GetDefaultFacets(string customerEndpoint, SearchIndexQueryCredentials queryCredentials)
        {
            SearchRequest request = new SearchRequest
            {
                SearchTerm = "",
                Filters = "",
                OrderBy = null,
                PageNumber = 0,
                PageSize = 0,
                NumberOfExistingResults = 0,
                CustomerEndpoint = customerEndpoint,
            };

            var data = await InvokeSearchRequest(request, queryCredentials);
            return data.SearchProductResult.Facets;
        }

        private void ValidateRequest(SearchRequest request, SearchIndexQueryCredentials queryCredentials)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (queryCredentials == null) throw new ArgumentNullException(nameof(queryCredentials));
        }

        private SearchClient GetSearchClient(SearchIndexQueryCredentials queryCredentials)
        {
            return _searchClientProvider.GetSearchClient(queryCredentials.search_instance_endpoint, queryCredentials.search_index_name, queryCredentials.QueryApiKey);
        }

        public async Task<IEnumerable<string>> AutocompleteWithSuggestions(string searchTerm, SearchIndexQueryCredentials queryCredentials)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            try
            {
                var searchClient = GetSearchClient(queryCredentials);
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