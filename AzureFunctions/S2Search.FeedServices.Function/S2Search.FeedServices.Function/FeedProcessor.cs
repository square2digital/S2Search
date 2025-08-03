using System;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace S2Search.FeedServices.Function
{
    public class FeedProcessor
    {
        private readonly IFeedMapperProvider feedMapperProvider;
        private readonly ISearchIndexCredentialsRepository searchIndexCredentialsRepo;
        private readonly IFeedRepository feedRepo;
        private readonly IFeedProcessingManager feedProcessingManager;

        public FeedProcessor(IFeedMapperProvider feedMapperProvider,
                             ISearchIndexCredentialsRepository searchIndexCredentialsRepo,
                             IFeedRepository feedRepo,
                             IFeedProcessingManager feedProcessingManager)
        {
            this.feedMapperProvider = feedMapperProvider ?? throw new ArgumentNullException(nameof(feedMapperProvider));
            this.searchIndexCredentialsRepo = searchIndexCredentialsRepo ?? throw new ArgumentNullException(nameof(searchIndexCredentialsRepo));
            this.feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
            this.feedProcessingManager = feedProcessingManager ?? throw new ArgumentNullException(nameof(feedProcessingManager));
        }

        [FunctionName(FunctionNames.FeedProcessor)]
        public async Task Run([QueueTrigger(StorageQueues.Process, Connection = ConnectionStrings.AzureStorageAccount)] FeedBlob feedBlob,
                                IBinder binder,
                                ILogger logger)
        {
            logger.LogInformation($"Processor | Processing FeedBlob - CustomerId: {feedBlob.CustomerId}");

            try
            {
                var customerId = Guid.Parse(feedBlob.CustomerId);
                var searchIndexProcessingData = await feedRepo.GetSearchIndexFeedProcessingData(customerId, feedBlob.SearchIndexName);
                var feedMapper = feedMapperProvider.GetMapper(searchIndexProcessingData.SearchIndexData.FeedDataFormat);
                var csvBlob = await binder.BindAsync<CloudBlockBlob>(new BlobAttribute(feedBlob.BlobUri));
                var feedData = await feedMapper.GetDataAsync(csvBlob);

                await feedProcessingManager.CreateOrUpdateIndexSuggesterAsync(searchIndexProcessingData.SearchIndexCredentials);
                await feedProcessingManager.CreateOrUpdateIndexSynonymsAsync(searchIndexProcessingData.SearchIndexCredentials);
                await feedProcessingManager.ProcessFeedDataAsync(feedData, searchIndexProcessingData.SearchIndexCredentials);

                var updateFeedRepoTask = feedProcessingManager.UpdateFeedRepositoryAsync(feedData, searchIndexProcessingData.SearchIndexCredentials);
                var moveCsvBlobTask = feedProcessingManager.MoveCsvBlobAsync(csvBlob, feedBlob);

                await Task.WhenAll(updateFeedRepoTask, moveCsvBlobTask);
                
                var purgeCacheMessage = CreatePurgeCacheMessage(searchIndexProcessingData);
                var queue = await binder.BindAsync<IAsyncCollector<PurgeCacheMessage>>(
                    new QueueAttribute(queueName: StorageQueues.PurgeCache)
                    {
                        Connection = ConnectionStrings.AzureStorageAccount
                    });

                await queue.AddAsync(purgeCacheMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Processor | Error: {ex.Message}");
                throw;
            }
        }

        private static PurgeCacheMessage CreatePurgeCacheMessage(SearchIndexFeedProcessingData searchIndexProcessingData)
        {
            return new PurgeCacheMessage()
            {
                Host = searchIndexProcessingData.SearchIndexData.SearchEndpoint
            };
        }
    }
}
