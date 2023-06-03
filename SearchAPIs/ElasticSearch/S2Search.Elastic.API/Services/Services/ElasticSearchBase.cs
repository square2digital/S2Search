using Domain.Enums;
using Domain.Extentions;
using Domain.Interfaces;
using Domain.Models.Facets;
using Domain.Models.Request;
using Domain.Models.Response.Generic;
using Domain.Models.Response.Vehicle;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Services.Helpers;
using Services.Interfaces.FacetOverrides;

namespace Services.Services
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
                    await BuildFacets(request, indexSchema);

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
                    await BuildFacets(request, indexSchema);

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

                if(aggregations != null && aggregations.Count > 0)
                {
                    response = await client.SearchAsync<T>(x => x
                                           .Index(request.Index)
                                           .Aggregations(aggregations)
                                           .Query(q => q
                                           .QueryString(s => s.Query(request.SearchTerm)))
                                           .Sort(s => s
                                               .Field(f => SortFromRequest(request))
                                            )
                                           .From(request.From)
                                           .Size(request.Size));
                }
                else
                {
                    response = await client.SearchAsync<T>(x => x
                                           .Index(request.Index)
                                           .Query(q => q
                                           .QueryString(s => s.Query(request.SearchTerm)))
                                           .Sort(s => s
                                               .Field(f => SortFromRequest(request))
                                            )
                                           .From(request.From)
                                           .Size(request.Size));
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
                _logger.LogError(ex, $"Error on SearchAsyncResponse | Message: {ex.Message}");
                _logger.LogInformation($"searchQuery {JsonConvert.SerializeObject(request.SearchTerm)}");

                throw;
            }
        }

        private async Task BuildFacets(SearchDataRequest request, string indexSchema)
        {
            // #######################
            // for testing purposes
            // #######################
            _indexMapping = string.Empty;
            _elasticFacets.Clear();
            _aggregations.Clear();

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

                facetGroup.FacetKey = kvp.Key;
                facetGroup.FacetName = kvp.Key;
                facetGroup.FacetKeyDisplayName = kvp.Key;

                foreach (var item in kvp.Value)
                {
                    facetGroup.FacetItems.Add(item.Value);
                }

                searchProductResult.Facets.Add(facetGroup);
            }
                            
            return searchProductResult;
        }

        private void BuildFacets(string index)
        {
            _elasticFacets = IndexFacetHelper.BuildGenericFacet(_indexMapping, index);

            foreach (var facet in _elasticFacets)
            {
                switch (facet.Type)
                {
                    case FacetType.terms:

                        var termsAggregation = new TermsAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Size = 10000
                        };

                        _aggregations.Add(facet.FacetName, new AggregationContainer { Terms = termsAggregation });

                        break;

                    case FacetType.range:

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

                    case FacetType.histogram:

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

        private async Task<SearchProductResult> InvokeVehicleFacetRequest(SearchDataRequest request)
        {
            try
            {
                if (_aggregations.Count == 0 || _elasticFacets.Count == 0)
                {
                    BuildVehicleFacets();
                }

                //create the search request
                var searchRequest = new SearchRequest
                {
                    QueryOnQueryString = request.SearchTerm,
                    From = request.From,
                    Size = request.Size,
                    Aggregations = _aggregations
                };

                var client = _elasticSearchClientProvider.GetElasticClient();
                ISearchResponse<SearchVehicle> result = await client.SearchAsync<SearchVehicle>(searchRequest);
                var response = new SearchProductResult();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(InvokeVehicleFacetRequest)} | Message: {ex.Message}");
                throw;
            }
        }

        private void BuildVehicleFacets()
        {
            // terms
            _elasticFacets.Add(new ElasticFacet("make", FacetType.terms, $"group_by_make_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "make.raw", null));
            _elasticFacets.Add(new ElasticFacet("model", FacetType.terms, $"group_by_model_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "model.raw", null));
            _elasticFacets.Add(new ElasticFacet("fuelType", FacetType.terms, $"group_by_fuelType_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "fuelType.raw", null));
            _elasticFacets.Add(new ElasticFacet("transmission", FacetType.terms, $"group_by_transmission_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "transmission.raw", null));
            _elasticFacets.Add(new ElasticFacet("engineSize", FacetType.terms, $"group_by_engineSize_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "engineSize.raw", null));
            _elasticFacets.Add(new ElasticFacet("colour", FacetType.terms, $"group_by_colour_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "colour.raw", null));
            _elasticFacets.Add(new ElasticFacet("year", FacetType.terms, $"group_by_year_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "year.raw", null));
            _elasticFacets.Add(new ElasticFacet("doors", FacetType.terms, $"group_by_doors_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "doors.raw", null));
            _elasticFacets.Add(new ElasticFacet("bodyStyle", FacetType.terms, $"group_by_bodyStyle_{Enum.GetName(typeof(FacetType), FacetType.terms)}", "bodyStyle.raw", null));

            // histogram
            _elasticFacets.Add(new ElasticFacet("price", FacetType.histogram, $"group_by_price_{Enum.GetName(typeof(FacetType), FacetType.histogram)}", "price.raw", null));
            _elasticFacets.Add(new ElasticFacet("monthlyPrice", FacetType.histogram, $"group_by_{Enum.GetName(typeof(FacetType), FacetType.histogram)}", "monthlyPrice.raw", null));

            // range
            _elasticFacets.Add(new ElasticFacet("price", FacetType.range, $"group_by_price_{Enum.GetName(typeof(FacetType), FacetType.range)}", "price.raw", 1000));
            _elasticFacets.Add(new ElasticFacet("monthlyPrice", FacetType.range, $"group_by_monthlyPrice_{Enum.GetName(typeof(FacetType), FacetType.range)}", "monthlyPrice.raw", 1000));

            foreach (var facet in _elasticFacets)
            {
                switch (facet.Type)
                {
                    case FacetType.terms:
                        var termsAggregation = new TermsAggregation(facet.FacetName)
                        {
                            Field = facet.Field,
                            Size = 10000
                        };
                        _aggregations.Add(facet.FacetName, new AggregationContainer { Terms = termsAggregation });
                        break;
                    case FacetType.histogram:
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
    }
}
