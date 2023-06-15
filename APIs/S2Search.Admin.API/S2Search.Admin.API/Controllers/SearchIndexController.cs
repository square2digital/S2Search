using Domain.Customer.SearchResources.CustomerPricing;
using Domain.Customer.SearchResources.SearchIndex;
using Microsoft.AspNetCore.Mvc;
using Services.Customer.Interfaces.Repositories;

namespace CustomerResource.Controllers
{
    [Route("api/customers/{customerId}/searchindex")]
    [ApiController]
    public class SearchIndexController : ControllerBase
    {
        private readonly ISearchIndexRepository _searchIndexRepo;
        private readonly ILogger _logger;

        public SearchIndexController(ISearchIndexRepository searchIndexRepo,
                                     ILogger<SearchIndexController> logger)
        {
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{searchIndexId}", Name = "GetSearchIndex")]
        [ProducesResponseType(typeof(SearchIndexFull), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid customerId, Guid searchIndexId, bool fullResource = false)
        {
            try
            {
                object searchIndex;

                if (fullResource)
                {
                    searchIndex = await _searchIndexRepo.GetFullAsync(customerId, searchIndexId);
                }
                else
                {
                    searchIndex = await _searchIndexRepo.GetAsync(customerId, searchIndexId);
                }

                if (searchIndex == null) return NotFound();
                return Ok(searchIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpGet("name/{friendlyName}", Name = "GetSearchIndexByName")]
        [ProducesResponseType(typeof(SearchIndex), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByName(Guid customerId, string friendlyName)
        {
            try
            {
                var searchIndex = await _searchIndexRepo.GetByFriendlyNameAsync(customerId, friendlyName);

                if (searchIndex == null || searchIndex.SearchIndexId == Guid.Empty)
                {
                    return NotFound();
                }

                return Ok(searchIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetByName)} | CustomerId: {customerId} | FriendlyName: {friendlyName} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpGet("pricing", Name = "GetSearchIndexPricing")]
        [ProducesResponseType(typeof(IEnumerable<CustomerPricingTier>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSearchIndexPricing(Guid customerId)
        {
            try
            {
                var results = await _searchIndexRepo.GetPricingTiers();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetSearchIndexPricing)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpPost("create", Name = "CreateSearchIndex")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(Guid customerId, [FromBody] SearchIndexRequest indexRequest)
        {
            try
            {
                var existingSearchIndex = await _searchIndexRepo.GetByFriendlyNameAsync(customerId, indexRequest.IndexName);

                if (existingSearchIndex != null && existingSearchIndex.SearchIndexId != Guid.Empty)
                {
                    return Conflict($"IndexName '{indexRequest.IndexName}' is already in use.");
                }

                await _searchIndexRepo.CreateAsync(indexRequest);

                return AcceptedAtAction(nameof(GetByName),
                                        ControllerContext.ActionDescriptor.ControllerName,
                                        new { customerId, friendlyName = indexRequest.IndexName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Create)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{searchIndexId}", Name = "DeleteSearchIndex")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var searchIndex = await _searchIndexRepo.GetAsync(customerId, searchIndexId);

                if (searchIndex == null) return NotFound();

                await _searchIndexRepo.DeleteAsync(searchIndexId);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Delete)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
