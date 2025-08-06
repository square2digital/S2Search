using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Controllers.Search.AzureCognitiveServices;
using S2Search.Backend.Domain.Search.Elastic.Interfaces;

namespace S2Search.Backend.Controllers.Search.Elastic
{
    [Route("v1/[controller]")]
    public class IndexController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IElasticIndexService _elasticIndexService;

        public IndexController(IElasticIndexService elasticIndexService,
                               ILogger<SearchController> logger,
                               IAppSettings appSettings) : base(appSettings, logger, elasticIndexService)
        {
            _elasticIndexService = elasticIndexService ?? throw new ArgumentNullException(nameof(elasticIndexService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        [HttpGet("UploadTestData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadTestData(string index)
        {
            try
            {
                // - delete the existing index
                await _elasticIndexService.DeleteIndex(index);

                // - create the new index
                await _elasticIndexService.CreateIndexFromSchemaUri(index, _appSettings.SearchSettings.DemoVehiclesIndexSchemaURL);

                // - import the data
                await _elasticIndexService.UploadTestVehicleDocuments(index, _appSettings.SearchSettings.DemoVehiclesURL);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(UploadTestData)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteIndex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteIndex(string index)
        {
            try
            {
                await _elasticIndexService.DeleteIndex(index);

                return Ok($"Successfully Delete index: {index}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(DeleteIndex)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetIndexSchema")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIndexSchema(string index)
        {
            if (string.IsNullOrEmpty(index))
            {
                return BadRequest();
            }

            try
            {
                var indexSchema = await _elasticIndexService.GetIndexSchema(index);
                Response.StatusCode = StatusCodes.Status200OK;
                return Content(indexSchema, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetIndexSchema)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetTotalIndexCount")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalIndexCount(string index)
        {
            if (string.IsNullOrEmpty(index))
            {
                return BadRequest();
            }

            try
            {
                var totalCount = await _elasticIndexService.GetTotalIndexCount(index);
                Response.StatusCode = StatusCodes.Status200OK;
                return Ok(totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetTotalIndexCount)} | Message: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
