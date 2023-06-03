using Domain.Interfaces;
using Domain.Models.Request;
using Domain.Models.Response.Generic;
using Domain.Models.Response.Vehicle;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Services.Interfaces.FacetOverrides;

namespace Services.Services
{
    public class ElasticSearchService : ElasticSearchBase, IElasticSearchService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IDisplayTextFormatHelper _displayTextFormatHelper;
        private readonly IElasticSearchClientProvider _elasticSearchClientProvider;
        private readonly ILuceneSyntaxHelper _luceneSyntaxHelper;
        private readonly IElasticIndexService _elasticIndexService;
        private readonly IFacetHelper _facetHelper;

        public ElasticSearchService(IAppSettings appSettings,
                                    ILoggerFactory loggerFactory,
                                    IElasticSearchClientProvider elasticSearchClientProvider,
                                    IDisplayTextFormatHelper displayTextFormatHelper,
                                    ILuceneSyntaxHelper luceneSyntaxHelper,
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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _displayTextFormatHelper = displayTextFormatHelper ?? throw new ArgumentNullException(nameof(displayTextFormatHelper));
            _elasticSearchClientProvider = elasticSearchClientProvider ?? throw new ArgumentNullException(nameof(elasticSearchClientProvider));
            _luceneSyntaxHelper = luceneSyntaxHelper ?? throw new ArgumentNullException(nameof(luceneSyntaxHelper));
            _elasticIndexService = elasticIndexService ?? throw new ArgumentNullException(nameof(elasticIndexService));
            _facetHelper = facetHelper ?? throw new ArgumentNullException(nameof(facetHelper));
        }

        public async Task<ClusterHealthResponse> HealthCheck()
        {
            try
            {
                var client = _elasticSearchClientProvider.GetElasticClient();
                var healthResponse = await client.Cluster.HealthAsync();

                if (!healthResponse.IsValid)
                {
                    _logger.LogInformation($"Health Debug Information: {healthResponse.DebugInformation}");
                }

                _logger.LogInformation($"healthResponse: {JsonConvert.SerializeObject(healthResponse, Formatting.Indented)}");

                return healthResponse;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error on SearchHealthCheck");
                _logger.LogError(ex, ex.ToString());

                throw;
            }
        }

        public async Task<PingResponse> PingCheck()
        {
            try
            {
                var client = _elasticSearchClientProvider.GetElasticClient();
                var pingResponse = await client.PingAsync();

                if (!pingResponse.IsValid)
                {
                    _logger.LogInformation($"Ping Debug Information: {pingResponse.DebugInformation}");
                }

                _logger.LogInformation($"pingResponse: {JsonConvert.SerializeObject(pingResponse, Formatting.Indented)}");

                return pingResponse;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error on {nameof(PingCheck)}");
                _logger.LogError(ex, ex.ToString());

                throw;
            }
        }

        public async Task<SearchProductResult> InvokeSearch(SearchDataRequest request)
        {
            return await base.InvokeSearchRequest(request);
        }

        public async Task<int> TotalDocumentCount(string index)
        {
            if (string.IsNullOrEmpty(index))
            {
                throw new ArgumentNullException(nameof(index));
            }

            var client = _elasticSearchClientProvider.GetElasticClient();
            var countRequest = new CountRequest(Indices.Index(index));
            long countResponse = (await client.CountAsync(countRequest)).Count;

            return Convert.ToInt32(countResponse);
        }

        public async Task<List<string>> AutoCompleteWithSuggestions(string searchTerm, string index)
        {
            List<string> suggestions = new List<string>();

            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            var client = _elasticSearchClientProvider.GetElasticClient();

            var searchResponse = client.Search<SearchVehicle>(s => s
                .Index(index)
                .Suggest(su => su
                    .Completion("make", c1 => c1
                        .Field(f => f.make.Suffix("completion"))
                        .Prefix(searchTerm)
                        .Size(5)
                    )
                    .Completion("model", c2 => c2
                        .Field(f => f.model.Suffix("completion"))
                        .Prefix(searchTerm)
                        .Size(5)
                    )
                    .Completion("variant", c3 => c3
                        .Field(f => f.variant.Suffix("completion"))
                        .Prefix(searchTerm)
                        .Size(5)
                    )
                    .Completion("city", c4 => c4
                        .Field(f => f.city.Suffix("completion"))
                        .Prefix(searchTerm)
                        .Size(5)
                    )
                )
            );

            if (searchResponse.IsValid)
            {
                suggestions = new List<string>();

                foreach (var suggest in searchResponse.Suggest.Values)
                {
                    foreach (var options in suggest)
                    {
                        suggestions.AddRange(options.Options.Select(x => x.Text));
                    }
                }

                suggestions = suggestions.Distinct().ToList();
            }

            return suggestions;
        }

        private string GetPrefixFromTerm(string searchTerm)
        {
            return searchTerm.Substring(0, 2);
        }
    }
}
