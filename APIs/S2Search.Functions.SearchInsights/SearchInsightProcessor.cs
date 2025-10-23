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

    public SearchInsightProcessor(ISearchInsightsManager searchInsightsManager,
                                  IDataPointsExtractionManager dataPointsExtractionManager)
    {
        this.searchInsightsManager = searchInsightsManager ?? throw new ArgumentNullException(nameof(searchInsightsManager));
        this.dataPointsExtractionManager = dataPointsExtractionManager ?? throw new ArgumentNullException(nameof(dataPointsExtractionManager));
    }

    [Function(nameof(SearchInsightProcessor))]
    public async Task Run([QueueTrigger(StorageQueues.SearchInsightsProcessing, Connection = ConnectionStrings.AzureStorageAccount)] SearchInsightMessage searchInsightMessage,
                               ILogger log)
    {
        log.LogInformation($"{nameof(SearchInsightProcessor)} | Processing Message - SearchIndexId: {searchInsightMessage.SearchIndexId}");
        log.LogInformation($"{searchInsightMessage}");

        var dataPoints = dataPointsExtractionManager.Extract(searchInsightMessage);

        await searchInsightsManager.SaveInsightsAsync(searchInsightMessage.SearchIndexId, dataPoints, searchInsightMessage.DateGenerated);
    }
}