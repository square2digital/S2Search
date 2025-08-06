using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Admin.Customer.Models;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/searchinsights")]
    [ApiController]
    public class SearchInsightsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISearchInsightsRetrievalManager _searchInsightsManager;

        public SearchInsightsController(ILogger<SearchInsightsController> logger,
                                        ISearchInsightsRetrievalManager searchInsightsManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _searchInsightsManager = searchInsightsManager ?? throw new ArgumentNullException(nameof(searchInsightsManager));
        }

        [HttpGet("summary", Name = nameof(GetSearchInsightsSummary))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchInsightSummary))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetSearchInsightsSummary([FromRoute] Guid customerId, [FromRoute] Guid searchIndexId)
        {
            try
            {
                var summaryData = await _searchInsightsManager.GetSummaryAsync(searchIndexId);
                return Ok(summaryData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetSearchInsightsSummary)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpGet("chart/{reportName}", Name = nameof(GetChartByReportName))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchInsightChart))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetChartByReportName([FromRoute] Guid customerId,
                                                             [FromRoute] Guid searchIndexId,
                                                             [FromRoute] string reportName,
                                                             [FromQuery] DateTime dateFrom,
                                                             [FromQuery] DateTime dateTo)
        {
            try
            {
                var result = await _searchInsightsManager.GetChartByNameAsync(searchIndexId,
                                                                              dateFrom,
                                                                              dateTo,
                                                                              reportName);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetChartByReportName)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Name: {reportName} | Message: {ex.Message}");
                throw;
            }
        }

        [HttpGet("tile/{reportName}", Name = nameof(GetTileByReportName))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchInsightTile))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTileByReportName([FromRoute] Guid customerId,
                                                            [FromRoute] Guid searchIndexId,
                                                            [FromRoute] string reportName,
                                                            [FromQuery] DateTime dateFrom,
                                                            [FromQuery] DateTime dateTo,
                                                            [FromQuery] bool includePreviousPeriod)
        {
            try
            {
                var result = await _searchInsightsManager.GetTileByNameAsync(searchIndexId,
                                                                             dateFrom,
                                                                             dateTo,
                                                                             reportName,
                                                                             includePreviousPeriod);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetTileByReportName)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Name: {reportName} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
