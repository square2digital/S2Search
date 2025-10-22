using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Helpers;
using Services.Interfaces.Managers;

namespace S2Search.FeedServices.Function
{
    public class FeedMonitor
    {
        private readonly IFeedTargetQueueManager _feedQueueManager;

        public FeedMonitor(IFeedTargetQueueManager feedQueueManager)
        {
            _feedQueueManager = feedQueueManager ?? throw new ArgumentNullException(nameof(feedQueueManager));
        }

        [FunctionName(FunctionNames.FeedMonitor)]
        public async Task Run([BlobTrigger(FeedAreas.BlobMonitorDirectory, Connection = ConnectionStrings.AzureStorageAccount)] CloudBlockBlob blockBlob,
                                IBinder binder,
                                string name,
                                ILogger log)
        {
            try
            {
                var blobPathSections = name.Split("/");
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
                    BlobUri = blockBlob.Uri.ToString(),
                    CurrentFeedArea = feedArea,
                    FileName = blobPathSections.Last(),
                    ManualUpload = manualUpload
                };

                feedBlob.SetNextDestination();

                string targetQueue = _feedQueueManager.GetTargetQueue(feedArea);

                var queue = await binder.BindAsync<IAsyncCollector<FeedBlob>>(
                    new QueueAttribute(queueName: targetQueue)
                    {
                        Connection = ConnectionStrings.AzureStorageAccount
                    });

                await queue.AddAsync(feedBlob);

                log.LogInformation($"Monitor | Blob added to queue for {customerId}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Monitor | Error: {ex.Message}");
                throw;
            }
        }
    }
}
