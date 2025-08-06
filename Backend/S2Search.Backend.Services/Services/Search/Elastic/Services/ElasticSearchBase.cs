using Azure.Search.Documents.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Objects;
using S2Search.Backend.Domain.Models.Response;
using S2Search.Backend.Services.Services.Search.Elastic.Helpers;
using Services.Providers;

namespace S2Search.Backend.Services.Services.Search.Elastic.Services
{
    public class ElasticSearchBase
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IDisplayTextFormatHelper _displayTextFormatHelper;
        private readonly IElasticSearchClientProvider _elasticSearchClientProvider;
        private readonly IElasticIndexService _elasticIndexService;

        private string _defaultSort = "desc";

        protected Dictionary<string, IAggregationContainer> _aggregations;
        protected List<ElasticFacet> _elasticFacets;
        private string _indexMapping = string.Empty;

        public ElasticSearchBase(IAppSettings appSettings,
                                  ILoggerFactory loggerFactory,
                                  IDisplayTextFormatHelper displayTextFormatHelper,
                                  IFacetHelper facetHelper,
                                  IFacetOverrideProvider facetOverrideProvider,
                                  IElasticIndexService elasticIndexService,
                                  IElasticSearchClientProvider elasticSearchClientProvider)
        {
            _logger = loggerFactory.CreateLogger<ElasticSearchBase>();
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _displayTextFormatHelper = displayTextFormatHelper ?? throw new ArgumentNullException(nameof(displayTextFormatHelper));
            _elasticSearchClientProvider = elasticSearchClientProvider ?? throw new ArgumentNullException(nameof(elasticSearchClientProvider));
            _elasticIndexService = elasticIndexService ?? throw new ArgumentNullException(nameof(elasticIndexService));

            _aggregations = new Dictionary<string, IAggregationContainer>();
            _elasticFacets = new List<ElasticFacet>();
        }

        protected async Task<SearchProductResult> InvokeSearchRequest(SearchDataRequest request)
        {
            SearchProductResult searchProductResult = new SearchProductResult();

            try
            {
                if (!_appSettings.UseGenericResponse)
                {
                    var indexSchema = await _elasticIndexService.GetIndexSchema(request.Index);
                    BuildFacets(request, indexSchema);

                    var client = _elasticSearchClientProvider.GetElasticClient();
                    var response = await SearchAsyncResponse<SearchVehicle>(client, request, _aggregations);

                    searchProductResult = BuildSearchProductResult<SearchVehicle>(response);
                    searchProductResult.Results = response.Documents.ToList();
                    searchProductResult.TotalResults = await _elasticIndexService.GetTotalIndexCount(request.Index);

                    // Get the request and response JSON
                    //var requestJson = ElasticExtensions.GetRequestJson(response);
                    //var responseJson = ElasticExtensions.GetResponseJson(response);
                }
                else
                {
                    var indexSchema = await _elasticIndexService.GetIndexSchema(request.Index);
                    BuildFacets(request, indexSchema);

                    var client = _elasticSearchClientProvider.GetElasticClient();
                    var response = await SearchAsyncResponse<GenericResponse>(client, request, _aggregations);
                    var responseJson = ElasticExtensions.GetResponseJson(response);

                    var genericResponse = GenericResponseHelper.BuildGenericResponse(responseJson, indexSchema);
                    searchProductResult = BuildSearchProductResult<GenericResponse>(response);
                    searchProductResult.GenericResults = genericResponse;
                    searchProductResult.TotalResults = await _elasticIndexService.GetTotalIndexCount(request.Index);
                }

                return searchProductResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on InvokeSearchRequest | Message: {ex.Message}");
                _logger.LogInformation($"searchQuery {JsonConvert.SerializeObject(request.SearchTerm)}");

                throw;
            }
        }

        private async Task<ISearchResponse<T>> SearchAsyncResponse<T>(ElasticClient client,
            SearchDataRequest request,
            Dictionary<string, IAggregationContainer> aggregations) where T : class
        {
            try
            {
                ISearchResponse<T> response;

                if (aggregations != null && aggregations.Count > 0)
                {
                    response = await client.SearchAsync<T>(x => x
                                           .Index(request.Index)
                                           .Aggregations(aggregations)
                                           .Query(q => q
                                           .QueryString(s => s.Query(BuildQueryString(request))))
                                           .Sort(s => s
                                               .Field(f => SortFromRequest(request))
                                            )
                                           .From(request.From)
                                           .Size(request.PageSize));
                }
                else
                {
                    response = await client.SearchAsync<T>(x => x
                                           .Index(request.Index)
                                           .Query(q => q
                                           .QueryString(s => s.Query(BuildQueryString(request))))
                                           .Sort(s => s
                                               .Field(f => SortFromRequest(request))
                                            )
                                           .From(request.From)
                                           .Size(request.PageSize));
                }

                if (!response.IsValid)
                {
                    _logger.LogError($"Error on SearchAsyncResponse response is not valid");
                    throw response.OriginalException;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on SearchAsyncResponse | Request: {JsonConvert.SerializeObject(request, Formatting.Indented)} | Message: {ex.Message}");
                _logger.LogInformation($"searchQuery {JsonConvert.SerializeObject(request.SearchTerm)}");

                throw;
            }
        }

        /// <summary>
        /// This needs to be unit tested
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string BuildQueryString(SearchDataRequest request)
        {
            var searchTerm = string.Empty;

            // ************************************
            // test for GenerateLuceneSearchString
            // ************************************
            searchTerm = LuceneSyntaxHelper.GenerateLuceneSearchString(request.SearchTerm);

            // ********************************************
            // this is what needs sorting for the filters
            // ********************************************

            if (!string.IsNullOrEmpty(request.Filters))
            {
                var testerSearchFilters = SearchFilterFormatterElastic.Format(request.Filters.Split(","));

                if (!string.IsNullOrEmpty(testerSearchFilters))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        searchTerm = $"{searchTerm} AND {testerSearchFilters}";
                    }
                    else
                    {
                        searchTerm = $"{testerSearchFilters}";
                    }
                }
            }

            return searchTerm;
        }

        private void BuildFacets(SearchDataRequest request, string indexSchema)
        {
            // #######################
            // for testing purposes
            // #######################
            //_indexMapping = string.Empty;
            //_elasticFacets.Clear();
            //_aggregations.Clear();

            _indexMapping = indexSchema;

            if (_aggregations.Count == 0 || _elasticFacets.Count == 0)
            {
                if (!_appSettings.UseGenericResponse)
                {
                    BuildVehicleFacets();
                }
                else
                {
                    BuildFacets(request.Index);
                }
            }
        }

        private SearchProductResult BuildSearchProductResult<T>(ISearchResponse<T> elasticResponse) where T : class
        {
            var searchProductResult = new SearchProductResult();
            var facets = new Dictionary<string, Dictionary<string, FacetItem>>();

            foreach (var key in elasticResponse.Aggregations.Keys)
            {
                Dictionary<string, FacetItem> data = new Dictionary<string, FacetItem>();

                // ********************
                // the facet type will need to be passed
                // catching exceptions will not be sustainable
                // ********************

                try
                {
                    if (elasticResponse.Aggregations.Terms(key) != null)
                    {
                        foreach (KeyedBucket<string>? item in elasticResponse.Aggregations.Terms(key).Buckets)
                        {
                            if (!data.ContainsKey(item.Key))
                            {
                                data.Add(item.Key, new FacetItem(item));
                            }
                        }
                    }
                }
                catch (Exception) { }

                try
                {
                    if (elasticResponse.Aggregations.Range(key) != null)
                    {
                        foreach (RangeBucket? item in elasticResponse.Aggregations.Range(key).Buckets)
                        {
                            if (!data.ContainsKey(item.Key))
                            {
                                data.Add(item.Key, new FacetItem(item));
                            }
                        }
                    }
                }
                catch (Exception) { }

                facets.Add(key, data);
            }

            foreach (var kvp in facets)
            {
                var facetGroup = new FacetGroup();

                facetGroup.FacetKey = GetFacetKeyFromFacetName(kvp.Key);
                facetGroup.FacetName = kvp.Key;
                facetGroup.FacetKeyDisplayName = kvp.Key;
                facetGroup.Type = GetFacetType(facetGroup.FacetKey);

                foreach (var item in kvp.Value)
                {
                    item.Value.Type = facetGroup.Type;
                    facetGroup.FacetItems.Add(item.Value);
                }

                searchProductResult.Facets.Add(facetGroup);
            }

            return searchProductResult;
        }

        private string GetFacetKeyFromFacetName(string facetName)
        {
            var facet = _elasticFacets.FirstOrDefault(x => x.FacetName == facetName);

            if (facet != null)
            {
                return facet.FacetKey;
            }

            return facetName;
        }

        private void BuildFacets(string index)
        {
            _elasticFacets = IndexFacetHelper.BuildGenericFacet(_indexMapping, index);

            foreach (var facet in _elasticFacets)
            {
                switch (facet.Type)
                {
                    case nameof(FacetType.terms):

                        var termsAggregation = new TermsAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Size = 10000
                        };

                        _aggregations.Add(facet.FacetName, new AggregationContainer { Terms = termsAggregation });

                        break;

                    case nameof(FacetType.range):

                        var list = new List<IAggregationRange>();

                        var to = 100000;
                        var interval = facet.Interval ?? 1000;
                        var count = Convert.ToInt32(to / interval) + 1;

                        for (int i = 0; i < count; i++)
                        {
                            var range = new AggregationRange();

                            if (i == 0)
                            {
                                range.From = i;
                                range.To = interval;
                            }
                            else
                            {
                                if (i == 1)
                                {
                                    range.From = Convert.ToInt32(i * interval);
                                    range.To = Convert.ToInt32((i + 1) * interval);
                                }
                                else
                                {
                                    range.From = Convert.ToInt32((i - 1) * interval);
                                    range.To = Convert.ToInt32(i * interval);
                                }
                            }

                            range.Key = $"{facet.Field} {_displayTextFormatHelper.FormatCurrencyRange(range.From.ToString(), range.To.ToString(), "£", string.Empty)}";

                            list.Add(range);
                        }

                        list = list.GroupBy(x => x.From)
                            .Select(x => x.First())
                            .ToList();

                        var rangeAggregation = new RangeAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Ranges = list
                        };

                        _aggregations.Add(facet.FacetName, new AggregationContainer { Range = rangeAggregation });

                        break;

                    case nameof(FacetType.histogram):

                        var histogramAggregation = new HistogramAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Interval = 5000
                        };

                        _aggregations.Add(facet.FacetName, new AggregationContainer { Histogram = histogramAggregation });

                        break;
                }
            }
        }

        private void BuildVehicleFacets()
        {
            // terms
            _elasticFacets.Add(new ElasticFacet("make", "Make", FacetType.terms.ToString(), $"group_by_make_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "make.raw", null, null));
            _elasticFacets.Add(new ElasticFacet("model", "Model", FacetType.terms.ToString(), $"group_by_model_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "model.raw", null, null));
            _elasticFacets.Add(new ElasticFacet("fuelType", "Fuel Type", FacetType.terms.ToString(), $"group_by_fuelType_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "fuelType.raw", null, null));
            _elasticFacets.Add(new ElasticFacet("transmission", "Transmission", FacetType.terms.ToString(), $"group_by_transmission_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "transmission.raw", null, null));
            _elasticFacets.Add(new ElasticFacet("colour", "Colour", FacetType.terms.ToString(), $"group_by_colour_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "colour.raw", null, null));
            _elasticFacets.Add(new ElasticFacet("year", "Year", FacetType.terms.ToString(), $"group_by_year_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "year", null, null));
            _elasticFacets.Add(new ElasticFacet("doors", "Doors", FacetType.terms.ToString(), $"group_by_doors_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "doors", null, null));
            _elasticFacets.Add(new ElasticFacet("bodyStyle", "Body Style", FacetType.terms.ToString(), $"group_by_bodyStyle_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "bodyStyle.raw", null, null));

            // range
            _elasticFacets.Add(new ElasticFacet("engineSize", "Engine Size", FacetType.range.ToString(), $"group_by_engineSize_{Enum.GetName(typeof(FacetType), FacetType.range)}", "engineSize", 500, 8000));
            _elasticFacets.Add(new ElasticFacet("price", "Price", FacetType.range.ToString(), $"group_by_price_{Enum.GetName(typeof(FacetType), FacetType.range)}", "price", 1000, 100000));
            _elasticFacets.Add(new ElasticFacet("monthlyPrice", "Monthly Price", FacetType.range.ToString(), $"group_by_monthlyPrice_{Enum.GetName(typeof(FacetType), FacetType.range)}", "monthlyPrice", 250, 10000));

            foreach (var facet in _elasticFacets)
            {
                switch (facet.Type)
                {
                    case nameof(FacetType.terms):
                        var termsAggregation = new TermsAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Size = 10000
                        };

                        if (!_aggregations.ContainsKey(facet.FacetName))
                        {
                            _aggregations.Add(facet.FacetName, new AggregationContainer { Terms = termsAggregation });
                        }

                        break;

                    case nameof(FacetType.range):

                        IList<IAggregationRange> ranges = new List<IAggregationRange>();

                        int rangeCount = facet.MaxValue.Value / facet.Interval.Value;
                        for (int i = 0; i < rangeCount; i++)
                        {
                            int from = i * facet.Interval.Value;
                            int to = (i + 1) * facet.Interval.Value;
                            var range = new AggregationRange
                            {
                                From = from,
                                To = to,
                            };

                            if(facet.FacetName == "Engine Size")
                            {
                                range.Key = $"{from:N0}cc - {to:N0}cc";
                            }
                            else
                            {
                                range.Key = $"£{from:N0} - £{to:N0}";
                            }

                            ranges.Add(range);
                        }

                        var rangeAggregation = new RangeAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Ranges = ranges.OrderByDescending(r => r.Key)
                        };

                        _aggregations.TryAdd(facet.FacetName, new AggregationContainer { Range = rangeAggregation });

                        break;
                }
            }
        }


        private IFieldSort SortFromRequest(SearchDataRequest request)
        {
            var fieldSort = GetFieldSort(request.OrderBy, request.SortOrder);

            if (request.SortOrder == _defaultSort)
            {
                fieldSort.Order = SortOrder.Descending;
            }
            else
            {
                fieldSort.Order = SortOrder.Ascending;
            }

            return fieldSort;
        }

        private FieldSort GetFieldSort(string orderBy, string sortBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "make.raw";
            }

            var fieldSort = new FieldSort();
            var field = new Field(orderBy);

            fieldSort.Field = field;

            switch (sortBy)
            {
                case "asc":
                    fieldSort.Order = SortOrder.Ascending;
                    break;

                default:
                    fieldSort.Order = SortOrder.Descending;
                    break;
            }

            return fieldSort;
        }

        private string GetFacetType(string FacetKey)
        {
            foreach(var item in _elasticFacets)
            {
                if(item.FacetKey == FacetKey)
                {
                    return item.Type;
                }
            }

            throw new Exception($"FacetKey: {FacetKey} is not found in the _elasticFacets collection");
        }
    }
}
