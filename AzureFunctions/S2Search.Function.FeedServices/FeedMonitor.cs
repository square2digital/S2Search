using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Constants;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using Services.Interfaces.Managers;
using System.Text.Json;

namespace S2Search.Function.FeedServices;

public class FeedMonitor
{
    private readonly ILogger<FeedMonitor> _logger;
    private readonly IFeedTargetQueueManager _feedQueueManager;

    public FeedMonitor(IFeedTargetQueueManager feedQueueManager, ILogger<FeedMonitor> logger)
    {
        _feedQueueManager = feedQueueManager ?? throw new ArgumentNullException(nameof(feedQueueManager));
        _logger = logger;
    }

    [Function(nameof(FeedMonitor))]
    public async Task Run([BlobTrigger(FeedAreas.BlobMonitorDirectory, Source = BlobTriggerSource.EventGrid)] 
        BlobClient blobClient,
        string name,
        ILogger log)
    {
        try
        {
            var blobPathSections = name.Split('/');
            string feedArea = blobPathSections[0];
            string customerId = blobPathSections[1];
            string searchIndexName = blobPathSections[2];
            bool manualUpload = name.Contains("/manualupload/");

            log.LogInformation($"Monitor | Blob detected {name} in {feedArea} for customerId: {customerId}");

            if (manualUpload)
            {
                log.LogInformation($"Monitor | This blob has been manually uploaded");
            }

            var feedBlob = new FeedBlob()
            {
                CustomerId = customerId,
                SearchIndexName = searchIndexName,
                BlobUri = blobClient.Uri.ToString(),
                CurrentFeedArea = feedArea,
                FileName = blobPathSections.Last(),
                ManualUpload = manualUpload
            };

            // Inline SetNextDestination logic
            string nextLocation = feedBlob.CurrentFeedArea switch
            {
                FeedAreas.Extract => FeedAreas.ValidateDirectory,
                FeedAreas.Validate => FeedAreas.ProcessDirectory,
                FeedAreas.Process => FeedAreas.CompletedDirectory,
                _ => "unknown",
            };

            string manualUploadPathPart = feedBlob.ManualUpload ? "/manualupload" : string.Empty;
            feedBlob.NextDestination = $"{nextLocation}/{feedBlob.CustomerId}/{feedBlob.SearchIndexName}{manualUploadPathPart}";

            string targetQueue = _feedQueueManager.GetTargetQueue(feedArea);

            // Get storage connection string from environment (the constant holds the env var name)
            var storageConnectionString = Environment.GetEnvironmentVariable(ConnectionStringKeys.AzureStorage);
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                throw new InvalidOperationException($"Storage connection string '{ConnectionStringKeys.AzureStorage}' is not configured.");
            }

            var queueClient = new QueueClient(storageConnectionString, targetQueue);
            await queueClient.CreateIfNotExistsAsync();

            var messageText = JsonSerializer.Serialize(feedBlob);
            await queueClient.SendMessageAsync(messageText);

            log.LogInformation($"Monitor | Blob added to queue for {customerId}");
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Monitor | Error: {ex.Message}");
            throw;
        }
    }
}
