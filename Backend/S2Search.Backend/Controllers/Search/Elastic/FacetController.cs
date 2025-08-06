using Domain.Models.Facets;
using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Controllers.Search.AzureCognitiveServices;
using S2Search.Backend.Controllers.Search.Elastic;
using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Search.Elastic.Interfaces;
using S2Search.Backend.Domain.Search.Elastic.Models.Request;
using S2Search.Backend.Domain.Search.Elastic.Models.Response.Facets;

namespace Search.Controllers
{
    [Route("v1/[controller]")]
    public class FacetController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IElasticFacetService _elasticFacetService;

        public FacetController(IElasticSearchService elasticSearchService,
                                IElasticFacetService elasticFacetService,
                                IElasticIndexService elasticIndexService,
                                ILogger<SearchController> logger,
                                IAppSettings appSettings) : base(appSettings, logger, elasticIndexService)
        {
            _elasticSearchService = elasticSearchService ?? throw new ArgumentNullException(nameof(elasticSearchService));
            _elasticFacetService = elasticFacetService ?? throw new ArgumentNullException(nameof(elasticFacetService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
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
        public async Task<IActionResult> Get([FromQuery] SearchDataRequest request)
        {
            try
            {
                LogApiDetails();
                ValidateIndex(request.Index, "DefaultFacets");

                var response = await _elasticFacetService.GetDefaultFacets(request.Index);
                return Ok(response.Facets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(IList<FacetGroup>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Facets([FromBody] SearchDataRequest request)
        {
            try
            {
                LogApiDetails();
                ValidateSearchDataRequest(request, "Facets");

                var response = await _elasticFacetService.GetSearchFacets(request);
                return Ok(response.Facets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Facets)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("DefaultFacets")]
        [ProducesResponseType(typeof(IList<FacetGroup>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DefaultFacets(string index)
        {
            try
            {
                LogApiDetails();
                ValidateIndex(index, "DefaultFacets");

                var response = await _elasticFacetService.GetDefaultFacets(index);
                return Ok(response.Facets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(DefaultFacets)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
