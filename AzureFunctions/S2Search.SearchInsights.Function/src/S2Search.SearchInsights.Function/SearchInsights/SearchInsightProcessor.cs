using System;
using Azure.Storage.Queues.Models;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;

namespace SearchInsightsNew
{
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
        public async Task Run([QueueTrigger(StorageQueues.SearchInsightsProcessing, Connection = ConnectionStrings.AzureStorageAccount)] QueueMessage message, SearchInsightMessage searchInsightMessage,
                               ILogger log)
        {
            log.LogInformation($"{nameof(SearchInsightProcessor)} | Processing Message - SearchIndexId: {searchInsightMessage.SearchIndexId}");
            log.LogInformation($"{searchInsightMessage}");

            var dataPoints = dataPointsExtractionManager.Extract(searchInsightMessage);

            await searchInsightsManager.SaveInsightsAsync(searchInsightMessage.SearchIndexId, dataPoints, searchInsightMessage.DateGenerated);
        }
    }
}
