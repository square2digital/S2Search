using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Domain.Models.Response;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.Backend.Domain.Interfaces;
using Services.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;

namespace S2Search.Backend.Controllers.Search.AzureCognitiveServices
{
    /// <summary>
    /// The Search Controller
    /// </summary>
    [Route("v1/[controller]")]
    public class SearchController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IAzureSearchService _azureSearchService;
        private readonly ISearchIndexQueryCredentialsProvider _queryCredentialsProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDistributedCacheService _redisService;
        private readonly IFireForgetService<IAzureQueueService> _fireForgetAzureQueueService;

        public SearchController(ILogger<SearchController> logger,
                                IAppSettings appSettings,
                                IAzureSearchService azureSearchService,
                                ISearchIndexQueryCredentialsProvider queryCredentialsProvider,
                                IHttpContextAccessor httpContext,
                                IDistributedCacheService redisService,
                                IFireForgetService<IAzureQueueService> fireForgetAzureQueueService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _azureSearchService = azureSearchService ?? throw new ArgumentNullException(nameof(azureSearchService));
            _queryCredentialsProvider = queryCredentialsProvider ?? throw new ArgumentNullException(nameof(queryCredentialsProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _fireForgetAzureQueueService = fireForgetAzureQueueService ?? throw new ArgumentNullException(nameof(fireForgetAzureQueueService));
        }

        /// <summary>
        /// Action method for search - POST
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SearchProductResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Search_Get")]
        public async Task<IActionResult> Get([FromQuery] SearchRequest request)
        {
            var result = new SearchResultRoot();

            if (request == null)
            {
                return BadRequest();
            }

            try
            {
                request.CallingHost = StringHelpers.FormatCallingHost(request.CallingHost);
                string redisKey = null;
                string redisValue = null;

                if (_appSettings.RedisCacheSettings.EnableRedisCache)
                {
                    try
                    {
                        redisKey = _redisService.CreateRedisKey(request.CallingHost, "search", HashHelper.GetXXHashString(JsonConvert.SerializeObject(request)));
                        redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Redis unavailable, skipping cache for search.");
                    }
                }

                if (redisValue != null)
                {
                    var searchResults = JsonConvert.DeserializeObject<SearchResultRoot>(redisValue);
                    LogSearchInsight(searchResults.SearchInsightMessage);
                    return Ok(searchResults.SearchProductResult);
                }

                var queryCredentials = await _queryCredentialsProvider.GetAsync(request.CallingHost);

                if (queryCredentials == null)
                {
                    return BadRequest("CallingHost not recognised.");
                }

                result = await _azureSearchService.InvokeSearchRequest(request, queryCredentials);
                Response.Headers.Add("Total-Results", result.SearchProductResult.TotalResults.ToString());

                if (_appSettings.RedisCacheSettings.EnableRedisCache && redisKey != null)
                {
                    try
                    {
                        await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(result), CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Redis unavailable, skipping cache set for search.");
                    }
                }

                LogSearchInsight(result.SearchInsightMessage);
                return Ok(result.SearchProductResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | Message: {ex}");
                _logger.LogInformation($"Search Request {JsonConvert.SerializeObject(request)}");
                _logger.LogInformation($"Search Product Result {JsonConvert.SerializeObject(result)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private void LogSearchInsight(SearchInsightMessage searchInsightMessage)
        {
            if (searchInsightMessage is null)
            {
                return;
            }

            _fireForgetAzureQueueService.Execute(async service =>
            {
                var searchInsightsMessage = service.CreateMessage(StorageQueues.SearchInsightsProcessing, searchInsightMessage);
                await service.EnqueueMessageAsync(searchInsightsMessage);
            });
        }

        [HttpGet("TotalDocumentCount/{callingHost}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Search_TotalDocumentCount")]
        public async Task<IActionResult> TotalDocumentCount(string callingHost)
        {
            if (string.IsNullOrEmpty(callingHost))
            {
                return BadRequest();
            }

            try
            {
                callingHost = StringHelpers.FormatCallingHost(callingHost);

                var queryCredentials = await _queryCredentialsProvider.GetAsync(callingHost);

                if (queryCredentials == null)
                {
                    return BadRequest("CallingHost not recognised.");
                }

                var total = await _azureSearchService.TotalDocumentCount(queryCredentials);
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(TotalDocumentCount)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("AutoSuggest/{searchTerm}/{callingHost}")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Search_Autocomplete")]
        public async Task<IActionResult> AutocompleteWithSuggestions(string searchTerm, string callingHost)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest();
            }

            try
            {
                if (_appSettings.RedisCacheSettings.EnableRedisCache)
                {

                    callingHost = StringHelpers.FormatCallingHost(callingHost);
                    var redisKey = _redisService.CreateRedisKey(callingHost, "autocomplete", HashHelper.GetXXHashString(JsonConvert.SerializeObject(searchTerm)));
                    var redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);

                    if (redisValue != null)
                    {
                        return Ok(JsonConvert.DeserializeObject<IEnumerable<string>>(redisValue));
                    }

                    var queryCredentials = await _queryCredentialsProvider.GetAsync(callingHost);
                    if (queryCredentials == null)
                    {
                        return BadRequest("CallingHost not recognised.");
                    }

                    var suggestions = await _azureSearchService.AutocompleteWithSuggestions(searchTerm, queryCredentials);

                    await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(suggestions), CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds));
                    return Ok(suggestions);
                }
                else
                {
                    var queryCredentials = await _queryCredentialsProvider.GetAsync(callingHost);
                    var suggestions = await _azureSearchService.AutocompleteWithSuggestions(searchTerm, queryCredentials);
                    return Ok(suggestions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(AutocompleteWithSuggestions)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}