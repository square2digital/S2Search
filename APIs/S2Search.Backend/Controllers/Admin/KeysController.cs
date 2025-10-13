using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/keys")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        private readonly ISearchIndexRepository _searchIndexRepo;
        private readonly IQueryKeyNameValidationManager _queryKeyNameManager;
        private readonly ILogger _logger;

        public KeysController(ISearchIndexRepository searchIndexRepo,
                              IQueryKeyNameValidationManager queryKeyNameManager,
                              ILogger<KeysController> logger)
        {
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
            _queryKeyNameManager = queryKeyNameManager ?? throw new ArgumentNullException(nameof(queryKeyNameManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost(Name = "DeleteKeys")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteKeysAsync(Guid customerId,
                                                         Guid searchIndexId,
                                                         [FromBody] SearchIndexKeyDeletionRequest keyDeletionRequest)
        {
            try
            {
                keyDeletionRequest.CustomerId = customerId;
                keyDeletionRequest.SearchIndexId = searchIndexId;

                await _searchIndexRepo.DeleteKeysAsync(keyDeletionRequest);
                return Accepted();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(DeleteKeysAsync)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
