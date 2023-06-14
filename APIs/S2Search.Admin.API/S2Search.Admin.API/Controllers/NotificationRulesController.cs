using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SearchResources.NotificationRules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Repositories;

namespace CustomerResource.Controllers
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/notificationrules")]
    [ApiController]
    public class NotificationRulesController : ControllerBase
    {
        private readonly INotificationRuleRepository _notificationRuleRepo;
        private readonly ILogger _logger;

        public NotificationRulesController(INotificationRuleRepository notificationRuleRepo,
                                           ILogger<NotificationRulesController> logger)
        {
            _notificationRuleRepo = notificationRuleRepo ?? throw new ArgumentNullException(nameof(notificationRuleRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all the notification rules for a search index
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        [HttpGet(Name = "GetNotificationRules")]
        [ProducesResponseType(typeof(IEnumerable<NotificationRule>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<NotificationRule>>> GetNotifications(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var results = await _notificationRuleRepo.GetByIdAsync(searchIndexId);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetNotifications)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets a specific notification rule by notificationId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="notificationRuleId"></param>
        [HttpGet("{notificationRuleId}", Name = "GetNotificationRuleById")]
        [ProducesResponseType(typeof(NotificationRule), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NotificationRule>> GetNotificationRule(Guid customerId, Guid searchIndexId, int notificationRuleId)
        {
            try
            {
                var result = await _notificationRuleRepo.GetByIdAsync(searchIndexId, notificationRuleId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetNotificationRule)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a notification rule for a search index
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="notificationRuleRequest"></param>
        [HttpPost(Name = "CreateNotificationRule")]
        [ProducesResponseType(typeof(NotificationRule), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid customerId, Guid searchIndexId, [FromBody] NotificationRuleRequest notificationRuleRequest)
        {
            try
            {
                notificationRuleRequest.SearchIndexId = searchIndexId;

                var result = await _notificationRuleRepo.CreateAsync(notificationRuleRequest);

                if (result?.NotificationRuleId == 0)
                {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(GetNotificationRule),
                                       new { customerId, searchIndexId, notificationRuleId = result.NotificationRuleId },
                                       result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(Post)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a notification rule
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="notificationRuleId"></param>
        [HttpDelete("{notificationRuleId}", Name = "DeleteNotificationRule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid customerId, Guid searchIndexId, int notificationRuleId)
        {
            try
            {
                await _notificationRuleRepo.DeleteAsync(searchIndexId, notificationRuleId);

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
