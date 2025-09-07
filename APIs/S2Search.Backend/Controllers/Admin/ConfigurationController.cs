using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Configuration.SearchResources;
using S2Search.Backend.Domain.Configuration.SearchResources.Configuration;
using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Configuration.SearchResources.Synonyms;
using S2Search.Backend.Domain.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISearchIndexRepository _searchIndexRepo;
        private readonly IThemeRepository _themeRepo;
        private readonly ISearchConfigurationRepository _searchUIConfigurationRepo;

        public ConfigurationController(ILogger<ConfigurationController> logger,
                                       ISearchIndexRepository searchIndexRepo,
                                       IThemeRepository themeRepo,
                                       ISearchConfigurationRepository searchUIConfigurationRepo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
            _themeRepo = themeRepo ?? throw new ArgumentNullException(nameof(themeRepo));
            _searchUIConfigurationRepo = searchUIConfigurationRepo ?? throw new ArgumentNullException(nameof(searchUIConfigurationRepo));
        }

        /// <summary>
        /// Retrieve the theme for the requested customerEndpoint
        /// </summary>
        /// <param name="customerEndpoint">The host that is calling the application consuming this endpoint.</param>
        [HttpGet("theme/{customerEndpoint}", Name = "GetTheme")]
        [ProducesResponseType(typeof(Theme), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTheme(string customerEndpoint)
        {
            try
            {
                var theme = await _themeRepo.GetThemeAsync(customerEndpoint);

                if (theme == null)
                {
                    _logger.LogInformation($"Not found on {nameof(GetTheme)} | CustomerEndpoint: {customerEndpoint}");
                    return NotFound();
                }

                return Ok(theme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetTheme)} | CustomerEndpoint: {customerEndpoint} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieve the Generic Synonyms
        /// </summary>
        /// <param name="category">The category of the generic synonyms to retrieve.</param>
        /// <returns></returns>
        [HttpGet("search/GenericSynonyms/{category}", Name = "GetGenericSynonyms")]
        [ProducesResponseType(typeof(IEnumerable<GenericSynonyms>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGenericSynonyms(string category = "vehicles")
        {
            try
            {
                var genericSynonyms = await _searchIndexRepo.GetGenericSynonymsByCategoryAsync(category);

                if (genericSynonyms == null)
                {
                    _logger.LogInformation($"Not found on {nameof(GetGenericSynonyms)} | Generic Synonyms Category: {category}");
                    return NotFound();
                }

                return Ok(genericSynonyms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetGenericSynonyms)} | Generic Synonyms Category: {category} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
