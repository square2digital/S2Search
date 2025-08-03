using Domain.Customer.SearchResources.SearchConfiguration;
using Domain.SearchResources.Configuration;
using Microsoft.AspNetCore.Mvc;
using Services.Configuration.Interfaces.Repositories;
using Services.Customer.Interfaces.Repositories;

namespace CustomerResource.Controllers
{
    [Route("api/customers/config")]
    [ApiController]
    public class ConfigController : Controller
    {        
        private readonly IThemeRepository _themeRepo;
        private readonly ISearchIndexRepository _searchIndexRepo;
        private readonly ISearchConfigurationRepository _searchConfigurationRepo;
        private readonly ILogger _logger;

        public ConfigController(IThemeRepository themeRepo,
            ISearchIndexRepository searchIndexRepo,
            ISearchConfigurationRepository searchConfigurationRepo,
            ILogger<QueryCredentialsController> logger)
        {
            _themeRepo = themeRepo ?? throw new ArgumentNullException(nameof(themeRepo));            
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
            _searchConfigurationRepo = searchConfigurationRepo ?? throw new ArgumentNullException(nameof(searchConfigurationRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the Search Configuration
        /// </summary>
        /// <param name="searchIndexId"></param>
        /// <returns></returns>
        [HttpGet("{searchIndexId}", Name = "GetConfig")]
        [ProducesResponseType(typeof(IEnumerable<SearchConfigurationOption>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid searchIndexId)
        {
            try
            {
                var response = await _searchConfigurationRepo.GetConfigurationForSearchIndexAsync(searchIndexId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Insert or Update a config item with a new value
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPut("update", Name = "UpdateConfig")]
        [ProducesResponseType(typeof(SearchConfigurationUpdateMapping), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateConfig([FromBody] SearchConfigurationUpdateMapping config)
        {
            try
            {
                var update = await _searchConfigurationRepo.UpdateConfigurationItem(config);

                if (update > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(UpdateConfig)} | ConfigId: {config.SearchConfigurationMappingId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
