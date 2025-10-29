using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Customer.SearchResources.Feeds;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/feeds")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly IFeedRepository _feedRepo;
        private readonly IFeedUploadManager _feedUploadManager;
        private readonly ILogger _logger;

        public FeedsController(IFeedRepository feedRepo,
                               IFeedUploadManager feedUploadManager,
                               ILogger<FeedsController> logger)
        {
            _feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
            _feedUploadManager = feedUploadManager ?? throw new ArgumentNullException(nameof(feedUploadManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the latest feed configuration for a search index
        /// </summary>
        [HttpGet("latest", Name = "GetLatestFeedConfiguration")]
        [ProducesResponseType(typeof(Feed), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Feed>> Get(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var result = await _feedRepo.GetLatestAsync(searchIndexId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Get)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a feed configuration entry and supersedes any previously existing feed configuration entries
        /// </summary>
        [HttpPost("create", Name = "SaveFeedConfiguration")]
        [ProducesResponseType(typeof(Feed), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Feed>> Post(Guid customerId, Guid searchIndexId, [FromBody] FeedRequest feedRequest)
        {
            feedRequest.SearchIndexId = searchIndexId;

            try
            {
                var result = await _feedRepo.CreateAsync(feedRequest);

                if (result == null || result == default(Feed))
                {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(Get), new { customerId, searchIndexId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Post)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Uploads a feed file for processing
        /// </summary>
        [HttpPost("upload", Name = "UploadFeedFile")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload(Guid customerId, Guid searchIndexId, IFormFile feedFile)
        {
            try
            {
                var uploadResponse = await _feedUploadManager.UploadFileAsync(customerId, searchIndexId, feedFile);

                if (!uploadResponse.Item1)
                {
                    _logger.LogInformation($"Bad request on {nameof(Upload)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {uploadResponse.Item2}");
                    return BadRequest(uploadResponse.Item2);
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Upload)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
