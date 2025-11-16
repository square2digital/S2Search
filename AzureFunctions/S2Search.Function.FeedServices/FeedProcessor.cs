using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System.Text.Json;

namespace S2Search.Function.FeedServices;

public class FeedProcessor
{
    private readonly IFeedMapperProvider feedMapperProvider;
    private readonly ISearchIndexCredentialsRepository searchIndexCredentialsRepo;
    private readonly IFeedServicesRepository feedRepo;
    private readonly IFeedProcessingManager feedProcessingManager;
    private readonly ILogger<FeedProcessor> _logger;

    public FeedProcessor(IFeedMapperProvider feedMapperProvider,
                             ISearchIndexCredentialsRepository searchIndexCredentialsRepo,
                             IFeedServicesRepository feedRepo,
                             IFeedProcessingManager feedProcessingManager,
                             ILogger<FeedProcessor> logger)
    {
        feedMapperProvider = feedMapperProvider ?? throw new ArgumentNullException(nameof(feedMapperProvider));
        searchIndexCredentialsRepo = searchIndexCredentialsRepo ?? throw new ArgumentNullException(nameof(searchIndexCredentialsRepo));
        feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
        feedProcessingManager = feedProcessingManager ?? throw new ArgumentNullException(nameof(feedProcessingManager));
        _logger = logger;
    }

    [Function(nameof(FeedProcessor))]
    public async Task Run([QueueTrigger(StorageQueues.Process)] FeedBlob feedBlob, ILogger logger)
    {
        _logger.LogInformation($"Processor | Processing FeedBlob - CustomerId: {feedBlob.CustomerId}");

        try
        {
            var customerId = Guid.Parse(feedBlob.CustomerId);
            var searchIndexProcessingData = await feedRepo.GetSearchIndexFeedProcessingData(customerId, feedBlob.SearchIndexName);
            var feedMapper = feedMapperProvider.GetMapper(searchIndexProcessingData.SearchIndexData.FeedDataFormat);

            // Use modern Azure SDK BlobClient instead of CloudBlockBlob
            var csvBlobClient = new BlobClient(new Uri(feedBlob.BlobUri));
            var feedData = await feedMapper.GetDataAsync(csvBlobClient);

            await feedProcessingManager.CreateOrUpdateIndexSuggesterAsync(searchIndexProcessingData.SearchIndexCredentials);
            await feedProcessingManager.CreateOrUpdateIndexSynonymsAsync(searchIndexProcessingData.SearchIndexCredentials);
            await feedProcessingManager.ProcessFeedDataAsync(feedData, searchIndexProcessingData.SearchIndexCredentials);

            var updateFeedRepoTask = feedProcessingManager.UpdateFeedRepositoryAsync(feedData, searchIndexProcessingData.SearchIndexCredentials);
            var moveCsvBlobTask = feedProcessingManager.MoveCsvBlobAsync(csvBlobClient, feedBlob);

            await Task.WhenAll(updateFeedRepoTask, moveCsvBlobTask);

            var purgeCacheMessage = CreatePurgeCacheMessage(searchIndexProcessingData);

            // Enqueue purge cache message using Azure.Storage.Queues.QueueClient
            var storageConnectionString = Environment.GetEnvironmentVariable(ConnectionStringKeys.AzureStorage);
            if (string.IsNullOrWhiteSpace(storageConnectionString))
            {
                throw new InvalidOperationException($"Storage connection string '{ConnectionStringKeys.AzureStorage}' is not configured.");
            }

            var queueClient = new QueueClient(storageConnectionString, StorageQueues.PurgeCache);
            await queueClient.CreateIfNotExistsAsync();

            var messageText = JsonSerializer.Serialize(purgeCacheMessage);
            await queueClient.SendMessageAsync(messageText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Processor | Error: {ex.Message}");
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