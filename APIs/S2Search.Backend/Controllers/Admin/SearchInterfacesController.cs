using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/searchinterfaces")]
    [ApiController]
    public class SearchInterfacesController : ControllerBase
    {
        private readonly ISearchInterfaceRepository _searchInterfaceRepository;
        private readonly ILogger _logger;

        public SearchInterfacesController(ISearchInterfaceRepository searchInterfaceRepository,
                                          ILogger<SearchInterfacesController> logger)
        {
            _searchInterfaceRepository = searchInterfaceRepository ?? throw new ArgumentNullException(nameof(searchInterfaceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the latest search interface configuration for a search index
        /// </summary>
        [HttpGet("latest", Name = "GetLatestSearchInterface")]
        [ProducesResponseType(typeof(SearchInterface), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SearchInterface>> Get(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var result = await _searchInterfaceRepository.GetLatestAsync(searchIndexId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a search interface configuration entry and supersedes and previous entry
        /// </summary>
        [HttpPost(Name = "CreateSearchInterface")]
        [ProducesResponseType(typeof(SearchInterface), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SearchInterface>> Post(Guid customerId,
                                                              Guid searchIndexId,
                                                              [FromBody] SearchInterfaceRequest searchInterfaceRequest)
        {
            searchInterfaceRequest.SearchIndexId = searchIndexId;

            try
            {
                var result = await _searchInterfaceRepository.CreateAsync(searchInterfaceRequest);
                return CreatedAtAction(nameof(Get), new { customerId, searchIndexId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Post)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
