using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Customer.SearchResources.SearchConfiguration;
using S2Search.Backend.Domain.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/config")]
    [ApiController]
    public class ConfigurationController : Controller
    {        
        private readonly IThemeRepository _themeRepo;
        private readonly ISearchIndexRepository _searchIndexRepo;
        private readonly ISearchConfigurationRepository _searchConfigurationRepo;
        private readonly ILogger _logger;

        public ConfigurationController(IThemeRepository themeRepo,
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
