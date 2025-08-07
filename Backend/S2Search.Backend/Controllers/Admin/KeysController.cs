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

        [HttpGet("all", Name = "GetKeys")]
        [ProducesResponseType(typeof(IEnumerable<SearchIndexKeys>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SearchIndexKeys>>> GetKeys(Guid customerId, Guid searchIndexId)
        {
            try 
            {
                var result = await _searchIndexRepo.GetKeysAsync(customerId, searchIndexId);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetKeys)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpPost("create", Name = "CreateKeys")]
        [ProducesResponseType(typeof(IEnumerable<SearchIndexKeys>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(Guid customerId,
                                                Guid searchIndexId,
                                                [FromBody] SearchIndexKeyGenerationRequest keyGenerationRequest)
        {
            try
            {
                string nameErrorMessage = string.Empty;
                var namesInvalid = keyGenerationRequest.KeysToGenerate.Any(x => !_queryKeyNameManager.IsValid(x, out nameErrorMessage));

                if (namesInvalid && !string.IsNullOrEmpty(nameErrorMessage))
                {
                    return BadRequest($"One or more query key names are invalid. {nameErrorMessage}");
                }

                var existingKeys = await _searchIndexRepo.GetKeysAsync(customerId, searchIndexId);

                if (existingKeys.Any(x => keyGenerationRequest.KeysToGenerate.Select(y => y.ToLowerInvariant())
                                                                             .Contains(x.Name.ToLowerInvariant())))
                {
                    return BadRequest("Existing key name found");
                }

                keyGenerationRequest.CustomerId = customerId;
                keyGenerationRequest.SearchIndexId = searchIndexId;

                await _searchIndexRepo.CreateKeysAsync(keyGenerationRequest);

                return AcceptedAtAction(nameof(GetKeys), ControllerContext.ActionDescriptor.ControllerName, new { customerId, searchIndexId });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Create)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
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
