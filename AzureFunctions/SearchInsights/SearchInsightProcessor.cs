using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SearchInsights
{
    public class SearchInsightProcessor
    {
        private readonly ILogger<SearchInsightProcessor> _logger;

        public SearchInsightProcessor(ILogger<SearchInsightProcessor> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SearchInsightProcessor))]
        public void Run([QueueTrigger("searchinsights-process", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
