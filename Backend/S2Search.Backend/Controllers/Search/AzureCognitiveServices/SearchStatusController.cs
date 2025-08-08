using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Interfaces;

namespace Search.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchStatusController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;

        public SearchStatusController(ILogger<SearchStatusController> logger,
                                IAppSettings appSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Get()
        {
            return Ok("Ok!");
        }

        [HttpGet("GetConfig")]
        [ProducesResponseType(200)]
        public IActionResult GetConfig()
        {
            try
            {
                var appsettingsJson = JsonConvert.SerializeObject(_appSettings, Formatting.Indented);
                return Ok(appsettingsJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | Message: {ex}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
