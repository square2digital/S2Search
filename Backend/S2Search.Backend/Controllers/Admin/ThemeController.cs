using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Admin.Customer.SearchResources.Themes;
using S2Search.Backend.Domain.Configuration.SearchResources;
using S2Search.Backend.Services.Services.Admin.Configuration.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/theme")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly IThemeRepository _themeRepo;
        private readonly ILogger _logger;

        public ThemeController(IThemeRepository themeRepo,
                               ILogger<ThemeController> logger)
        {
            _themeRepo = themeRepo ?? throw new ArgumentNullException(nameof(themeRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the theme by the themeId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="themeId"></param>
        /// <returns></returns>
        [HttpGet("{themeId}", Name = "GetThemeById")]
        [ProducesResponseType(typeof(Theme), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Theme>> GetThemeById(Guid customerId, Guid searchIndexId, Guid themeId)
        {
            try
            {
                var result = await _themeRepo.GetThemeById(themeId);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetThemeById)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the theme associated to the Search Index
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetThemeBySearchIndexId")]
        [ProducesResponseType(typeof(Theme), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Theme>> GetThemeBySearchIndexId(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var result = await _themeRepo.GetThemeBySearchIndexId(searchIndexId);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetThemeBySearchIndexId)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets all themes associated to the CustomerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("all", Name = "GetThemesByCustomerId")]
        [ProducesResponseType(typeof(ThemeCollection), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ThemeCollection>> GetThemesByCustomerId(Guid customerId)
        {
            try
            {
                var result = await _themeRepo.GetThemesByCustomerId(customerId);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetThemesByCustomerId)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates Theme with new values
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpPut("update", Name = "UpdateTheme")]
        [ProducesResponseType(typeof(Theme), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateTheme(Guid customerId, Guid searchIndexId, [FromBody] ThemeRequest theme)
        {
            try
            {
                var result = await _themeRepo.GetThemeById(theme.themeId);

                if (result == null)
                {
                    return NotFound();
                }

                var update = await _themeRepo.UpdateTheme(theme);

                if (update > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(UpdateTheme)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
