using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Elastic.API.Controllers
{
    [Route("v1/[controller]")]
    public class HealthController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IElasticSearchService _elasticSearchService;

        public HealthController(IElasticSearchService elasticSearchService,
                                ILogger<HealthController> logger,
                                IAppSettings appSettings)
        {
            _elasticSearchService = elasticSearchService ?? throw new ArgumentNullException(nameof(elasticSearchService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        [HttpGet("HealthCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var healthCheck = await _elasticSearchService.HealthCheck();

                if (healthCheck.IsValid)
                {
                    var response = JsonConvert.SerializeObject(healthCheck);
                    return Ok(response);
                }

                throw new Exception(healthCheck.OriginalException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(HealthCheck)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("PingCheck")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PingCheck()
        {
            try
            {
                var pingCheck = await _elasticSearchService.PingCheck();
                
                if(pingCheck.IsValid)
                {
                    return Ok(pingCheck.DebugInformation);
                }

                throw new Exception(pingCheck.OriginalException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(PingCheck)} | Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
