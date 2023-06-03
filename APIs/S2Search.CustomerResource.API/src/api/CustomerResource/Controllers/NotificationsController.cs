using Domain.SearchResources.Notifications;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using System;
using System.Threading.Tasks;

namespace CustomerResource.Controllers
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationManager _notificationManager;
        private readonly ILogger _logger;
        private const int _maxPageSize = 25;

        public NotificationsController(INotificationManager notificationManager,
                                       ILogger<NotificationsController> logger)
        {
            _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all the notifications for a search index
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        [HttpGet(Name = "GetNotifications")]
        [ProducesResponseType(typeof(PagedResults<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResults<Notification>>> GetNotifications(Guid customerId, Guid searchIndexId, int page, int pageSize)
        {
            try
            {
                pageSize = pageSize > _maxPageSize ? _maxPageSize : pageSize;
                var rowsToSkip = (page - 1) * pageSize;
                var results = await _notificationManager.GetNotificationsPaged(searchIndexId, rowsToSkip, pageSize);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetNotifications)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
