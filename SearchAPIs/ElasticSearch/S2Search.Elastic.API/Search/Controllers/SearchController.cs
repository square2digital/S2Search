using Domain.Interfaces;
using Domain.Models.Request;
using Domain.Models.Response.Generic;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using Services.Helpers;
using Services.Interfaces;
using Services.Services;

namespace Elastic.API.Controllers
{
    [Route("v1/[controller]")]
    public class SearchController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IElasticFacetService _elasticFacetService;
        private readonly IElasticIndexService _elasticIndexService;

        public SearchController(IElasticSearchService elasticSearchService,
                                IElasticFacetService elasticFacetService,
                                IElasticIndexService elasticIndexService,
                                ILogger<SearchController> logger,
                                IAppSettings appSettings) : base(appSettings, logger, elasticIndexService)
        {
            _elasticSearchService = elasticSearchService ?? throw new ArgumentNullException(nameof(elasticSearchService));
            _elasticFacetService = elasticFacetService ?? throw new ArgumentNullException(nameof(elasticFacetService));
            _elasticIndexService = elasticIndexService ?? throw new ArgumentNullException(nameof(elasticIndexService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// GET search method from Azure Search API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SearchProductResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] SearchDataRequest request)
        {
            try
            {
                ValidateSearchDataRequest(request, "Search");

                var response = await _elasticSearchService.InvokeSearch(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | Message: {ex}");
                _logger.LogInformation($"Search Request {JsonConvert.SerializeObject(request)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(IReadOnlyCollection<GenericResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search([FromBody] SearchDataRequest request)
        {
            try
            {
                ValidateSearchDataRequest(request, "Search");

                var response = await _elasticSearchService.InvokeSearch(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Search)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("TotalDocumentCount/{index}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TotalDocumentCount(string index)
        {
            if (string.IsNullOrEmpty(index))
            {
                return BadRequest();
            }

            ValidateIndex(index, "TotalDocumentCount");

            try
            {
                var response = await _elasticSearchService.TotalDocumentCount(index);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(TotalDocumentCount)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("AutoSuggest/{index}")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AutoCompleteWithSuggestions(string searchTerm, string index)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest();
            }

            try
            {
                var suggestions = await _elasticSearchService.AutoCompleteWithSuggestions(searchTerm, index);
                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(AutoCompleteWithSuggestions)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
