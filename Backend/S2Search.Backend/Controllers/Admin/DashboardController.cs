using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.Admin.Customer.Dashboard;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardManager _dashboardManager;
        private readonly ILogger _logger;

        public DashboardController(IDashboardManager dashboardManager,
                                   ILogger<DashboardController> logger)
        {
            _dashboardManager = dashboardManager ?? throw new ArgumentNullException(nameof(dashboardManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the customers dashboard summary data
        /// </summary>
        [HttpGet("summary", Name = "GetDashboardSummary")]
        [ProducesResponseType(typeof(IEnumerable<DashboardSummaryItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DashboardSummaryItem>>> GetSummary(Guid customerId)
        {
            try
            {
                var results = await _dashboardManager.GetSummaryItems(customerId);

                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetSummary)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
