using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Managers;

namespace S2Search.Functions.SearchInsights;

public class SearchInsightProcessor
{
    private readonly ISearchInsightsManager searchInsightsManager;
    private readonly IDataPointsExtractionManager dataPointsExtractionManager;
    private readonly ILogger<SearchInsightProcessor> _logger;

    public SearchInsightProcessor(ISearchInsightsManager searchInsightsManager,
                                  IDataPointsExtractionManager dataPointsExtractionManager,
                                  ILogger<SearchInsightProcessor> logger)
    {
        this.searchInsightsManager = searchInsightsManager ?? throw new ArgumentNullException(nameof(searchInsightsManager));
        this.dataPointsExtractionManager = dataPointsExtractionManager ?? throw new ArgumentNullException(nameof(dataPointsExtractionManager));
        _logger = logger;
    }

    [Function(nameof(SearchInsightProcessor))]
    public async Task Run([QueueTrigger(StorageQueues.SearchInsightsProcessing, Connection = ConnectionStrings.AzureStorageAccount)] SearchInsightMessage searchInsightMessage)
    {
        _logger.LogInformation($"{nameof(SearchInsightProcessor)} | Processing Message - SearchIndexId: {searchInsightMessage.SearchIndexId}");
        _logger.LogInformation($"{searchInsightMessage}");

        var dataPoints = dataPointsExtractionManager.Extract(searchInsightMessage);

        await searchInsightsManager.SaveInsightsAsync(searchInsightMessage.SearchIndexId, dataPoints, searchInsightMessage.DateGenerated);
    }
}