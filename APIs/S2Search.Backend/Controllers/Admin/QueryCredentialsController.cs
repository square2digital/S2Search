using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryCredentialsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISearchIndexRepository _searchIndexRepo;

        public QueryCredentialsController(ILogger<QueryCredentialsController> logger,
                                          ISearchIndexRepository searchIndexRepo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
        }

        [HttpGet("endpoint/{customerEndpoint}")]
        [ProducesResponseType(typeof(SearchIndexQueryCredentials), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SearchIndexQueryCredentials>> GetQueryCredentials(string customerEndpoint)
        {
            try
            {
                var queryCredentials = await _searchIndexRepo.GetQueryCredentials(customerEndpoint);

                if (queryCredentials == null)
                {
                    _logger.LogInformation($"Not found on {nameof(GetQueryCredentials)} | CustomerEndpoint: {customerEndpoint}");
                    return NotFound();
                }

                return Ok(queryCredentials);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetQueryCredentials)} | CustomerEndpoint: {customerEndpoint} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
