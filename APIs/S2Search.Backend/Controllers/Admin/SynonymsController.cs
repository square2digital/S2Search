using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/synonyms")]
    [ApiController]
    public class SynonymsController : ControllerBase
    {
        private readonly ISynonymRepository _synonymRepo;
        private readonly ILogger _logger;

        public SynonymsController(ISynonymRepository synonymRepo,
                                  ISearchIndexRepository searchIndexRepo,
                                  ILogger<SynonymsController> logger)
        {
            _synonymRepo = synonymRepo ?? throw new ArgumentNullException(nameof(synonymRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all the synonyms for a search index
        /// </summary>
        [HttpGet(Name = "GetSynonyms")]
        [ProducesResponseType(typeof(IEnumerable<Synonym>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Synonym>>> GetSynonyms(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var results = await _synonymRepo.GetAsync(searchIndexId);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetSynonyms)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets a specific synonym by synonymId
        /// </summary>
        [HttpGet("{synonymId}", Name = "GetSynonymById")]
        [ProducesResponseType(typeof(Synonym), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Synonym>> GetSynonym(Guid customerId, Guid searchIndexId, Guid synonymId)
        {
            try
            {
                var result = await _synonymRepo.GetByIdAsync(searchIndexId, synonymId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetSynonym)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a synonym for a search index
        /// </summary>
        [HttpPost(Name = "CreateSynonym")]
        [ProducesResponseType(typeof(Synonym), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid customerId, Guid searchIndexId, [FromBody] SynonymRequest synonymRequest)
        {
            synonymRequest.SynonymId = Guid.NewGuid();
            synonymRequest.SearchIndexId = searchIndexId;

            try
            {
                var synonymExists = await _synonymRepo.GetByKeyWordAsync(searchIndexId, synonymRequest.KeyWord);

                if (synonymExists != null)
                {
                    return Conflict($"A synonym with the {nameof(synonymRequest.KeyWord)} of {synonymRequest.KeyWord} already exists");
                }

                var result = await _synonymRepo.CreateAsync(synonymRequest);

                if (result?.SynonymId == Guid.Empty)
                {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(GetSynonym),
                                       new { customerId, searchIndexId, synonymId = result.SynonymId },
                                       result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Post)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Update synonym
        /// </summary>
        [HttpPut("{synoynmId}", Name = "UpdateSynonym")]
        [ProducesResponseType(typeof(Synonym), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid customerId,
                                             Guid searchIndexId,
                                             Guid synoynmId,
                                             [FromBody] SynonymRequest synonymRequest)
        {
            synonymRequest.SearchIndexId = searchIndexId;
            synonymRequest.SynonymId = synoynmId;

            try
            {
                var synonymToUpdate = await _synonymRepo.GetByIdAsync(searchIndexId, synoynmId);

                if (synonymToUpdate == null)
                {
                    return NotFound();
                }

                var result = await _synonymRepo.UpdateAsync(synonymRequest);

                if (result?.SynonymId == Guid.Empty)
                {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(GetSynonym),
                                       new { customerId, searchIndexId, synonymId = result.SynonymId },
                                       result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Put)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a synonym
        /// </summary>
        [HttpDelete("{synonymId}", Name = "DeleteSynonym")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid customerId, Guid searchIndexId, Guid synonymId)
        {
            try
            {
                var result = await _synonymRepo.GetByIdAsync(searchIndexId, synonymId);

                if (result == null)
                {
                    return NotFound();
                }

                await _synonymRepo.DeleteAsync(searchIndexId, synonymId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Delete)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
