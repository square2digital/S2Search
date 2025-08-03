using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using Services.Helpers;

namespace FunctionsTest
{
    public class FeedMonitor
    {
        private readonly IFeedTargetQueueManager _feedQueueManager;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<FeedMonitor> _logger;

        public FeedMonitor(IFeedTargetQueueManager feedQueueManager,
            BlobServiceClient blobServiceClient,
            ILogger<FeedMonitor> logger)
        {
            _feedQueueManager = feedQueueManager ?? throw new ArgumentNullException(nameof(feedQueueManager));
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        [Function(nameof(FeedMonitor))]
        public async Task Run([BlobTrigger(FeedAreas.BlobMonitorDirectory, Connection = ConnectionStrings.AzureStorageAccount)] Stream stream, string name)
        {
            // Get Blob URL
            var blobClient = _blobServiceClient.GetBlobContainerClient("feed-services").GetBlobClient($"processing/{name}");
            string blobUrl = blobClient.Uri.ToString();
            _logger.LogInformation($"Blob URL: {blobUrl}");

            var blobPathSections = name.Split("/");
            string feedArea = blobPathSections[0];
            string customerId = blobPathSections[1];
            string searchIndexName = blobPathSections[2];
            bool manualUpload = name.Contains("/manualupload/");

            _logger.LogInformation($"Monitor | Blob detected {name} in {feedArea} for customerId: {customerId}");

            if (manualUpload)
            {
                _logger.LogInformation($"Monitor | This blob has been manually uploaded");
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

            feedBlob.SetNextDestination();

            string targetQueue = _feedQueueManager.GetTargetQueue(feedArea);

            // Replace IBinder with direct Azure SDK for queue management
            var blobConnectionString = Environment.GetEnvironmentVariable("AzureStorageAccount");
            var queueClient = new Azure.Storage.Queues.QueueClient(blobConnectionString, targetQueue);
            await queueClient.CreateIfNotExistsAsync();

            if (queueClient.Exists())
            {
                //await queueClient.SendMessageAsync(System.Text.Json.JsonSerializer.Serialize(feedBlob));

                var serializedMessage = System.Text.Json.JsonSerializer.Serialize(feedBlob);
                var encodedMessage = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serializedMessage));

                _logger.LogInformation($"Encoded message: {encodedMessage}");
                await queueClient.SendMessageAsync(encodedMessage);

            }

            _logger.LogInformation($"Monitor | Blob added to queue for {customerId}");

            using var blobStreamReader = new StreamReader(stream);
            {
                var content = await blobStreamReader.ReadToEndAsync();
                _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
            }
        }
    }
}
