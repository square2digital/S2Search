using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace FunctionsTest
{
    public class FeedProcessor
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IFeedMapperProvider feedMapperProvider;
        private readonly ISearchIndexCredentialsRepository searchIndexCredentialsRepo;
        private readonly IFeedRepository feedRepo;
        private readonly IFeedProcessingManager feedProcessingManager;
        private readonly ILogger<FeedProcessor> logger;

        public FeedProcessor(BlobServiceClient blobServiceClient, 
                            IFeedMapperProvider feedMapperProvider,
                            ISearchIndexCredentialsRepository searchIndexCredentialsRepo,
                            IFeedRepository feedRepo,
                            IFeedProcessingManager feedProcessingManager,
                            ILoggerFactory loggerFactory)
        {
            _blobServiceClient = blobServiceClient;
            this.feedMapperProvider = feedMapperProvider ?? throw new ArgumentNullException(nameof(feedMapperProvider));
            this.searchIndexCredentialsRepo = searchIndexCredentialsRepo ?? throw new ArgumentNullException(nameof(searchIndexCredentialsRepo));
            this.feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
            this.feedProcessingManager = feedProcessingManager ?? throw new ArgumentNullException(nameof(feedProcessingManager));
            this.logger = loggerFactory.CreateLogger<FeedProcessor>();
        }

        [Function(nameof(FeedProcessor))]
        public async Task Run([QueueTrigger(StorageQueues.Process, Connection = ConnectionStrings.AzureStorageAccount)] FeedBlob feedBlob,
            FunctionContext context)
        {
            logger.LogInformation($"Processor | Processing FeedBlob - CustomerId: {feedBlob.CustomerId}");

            try
            {
                var customerId = Guid.Parse(feedBlob.CustomerId);
                var searchIndexProcessingData = await feedRepo.GetSearchIndexFeedProcessingData(customerId, feedBlob.SearchIndexName);
                var feedMapper = feedMapperProvider.GetMapper(searchIndexProcessingData.SearchIndexData.FeedDataFormat);

                // Parse the container and blob name from the BlobUri
                var blobUri = new Uri(feedBlob.BlobUri);
                var containerName = blobUri.Segments[1].TrimEnd('/'); // Extracts container name
                var blobName = string.Join("", blobUri.Segments.Skip(2)); // Extracts the blob name (after the container)

                // Initialize BlobClient to access the blob
                //var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable(ConnectionStrings.AzureStorageAccount));
                //var blobContainerClient = blobServiceClient.GetBlobContainerClient(new Uri(feedBlob.BlobUri).Segments[1]);
                //var csvBlobClient = blobContainerClient.GetBlobClient(feedBlob.BlobUri);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var csvBlobClient = blobContainerClient.GetBlobClient(blobName);

                // Download the blob data
                var feedData = await feedMapper.GetDataAsync(csvBlobClient);

                // Process feed data and update index
                await feedProcessingManager.CreateOrUpdateIndexSuggesterAsync(searchIndexProcessingData.SearchIndexCredentials);
                await feedProcessingManager.CreateOrUpdateIndexSynonymsAsync(searchIndexProcessingData.SearchIndexCredentials);
                await feedProcessingManager.ProcessFeedDataAsync(feedData, searchIndexProcessingData.SearchIndexCredentials);

                // Perform tasks to update repository and move blob
                var updateFeedRepoTask = feedProcessingManager.UpdateFeedRepositoryAsync(feedData, searchIndexProcessingData.SearchIndexCredentials);
                var moveCsvBlobTask = feedProcessingManager.MoveCsvBlobAsync(csvBlobClient, feedBlob);

                await Task.WhenAll(updateFeedRepoTask, moveCsvBlobTask);

                // Initialize the QueueClient for purging cache
                var queueClient = new QueueClient(Environment.GetEnvironmentVariable(ConnectionStrings.AzureStorageAccount), StorageQueues.PurgeCache);
                await queueClient.CreateIfNotExistsAsync();

                if (queueClient.Exists())
                {
                    var purgeCacheMessage = CreatePurgeCacheMessage(searchIndexProcessingData);
                    var messageBody = System.Text.Json.JsonSerializer.Serialize(purgeCacheMessage);
                    await queueClient.SendMessageAsync(messageBody);
                }
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
