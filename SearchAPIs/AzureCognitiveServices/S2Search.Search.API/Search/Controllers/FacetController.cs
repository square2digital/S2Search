using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Domain.Models.Request;
using Domain.Models.Response;
using Domain.Models.Interfaces;
using Domain.Models.Facets;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Services.Helpers;
using Newtonsoft.Json;
using Services.Interfaces.Cache;

namespace Search.Controllers
{
    /// <summary>
    /// The Facet Controller
    /// </summary>
    [Route("v1/[controller]")]
    [ResponseCache(Duration = 10)]
    public class FacetController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IAzureFacetService _azureFacetService;
        private readonly ISearchIndexQueryCredentialsProvider _queryCredentialsProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAppSettings _appSettings;
        private readonly IDistributedCacheService _redisService;

        public FacetController(ILogger<FacetController> logger,
                               IAzureFacetService azureFacetService,
                               ISearchIndexQueryCredentialsProvider queryCredentialsProvider,
                               IHttpContextAccessor httpContext,
                               IAppSettings appSettings,
                               IDistributedCacheService redisService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azureFacetService = azureFacetService ?? throw new ArgumentNullException(nameof(azureFacetService));
            _queryCredentialsProvider = queryCredentialsProvider ?? throw new ArgumentNullException(nameof(queryCredentialsProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        }

        /// <summary>
        /// Action method for getting all facets from a Default Search
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(FacetedSearchResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Facets_Get")]
        public async Task<IActionResult> Get([FromQuery] SearchRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            try
            {
                var callingHost = StringHelpers.FormatCallingHost(request.CallingHost);
                var redisKey = _redisService.CreateRedisKey(callingHost, "facets", HashHelper.GetXXHashString(JsonConvert.SerializeObject(request)));
                var redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);

                if (redisValue != null)
                {
                    return Ok(JsonConvert.DeserializeObject<FacetedSearchResult>(redisValue));
                }                

                var queryCredentials = await _queryCredentialsProvider.GetAsync(callingHost);

                if (queryCredentials == null)
                {
                    return NotFound("CallingHost not recognised.");
                }

                IList<FacetGroup> facets = _azureFacetService.GetOrSetDefaultFacets(callingHost, queryCredentials);
                FacetedSearchResult result = new FacetedSearchResult(facets);

                await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(result), TimeSpan.FromSeconds(600));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}