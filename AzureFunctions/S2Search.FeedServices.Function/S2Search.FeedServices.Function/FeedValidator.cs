using System.Text;
using Azure.Storage.Blobs;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace FunctionsTest
{
    public class FeedValidator
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IFeedRepository _feedRepo;
        private readonly IFeedMapperProvider _feedMapperProvider;
        private readonly IFeedProcessingManager _feedProcessingManager;
        private readonly ILogger<FeedValidator> _logger;

        public FeedValidator(BlobServiceClient blobServiceClient,
                             IFeedRepository feedRepo,
                             IFeedMapperProvider feedMapperProvider,
                             IFeedProcessingManager feedProcessingManager,
                             ILoggerFactory loggerFactory)
        {
            _blobServiceClient = blobServiceClient;
            _feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
            _feedMapperProvider = feedMapperProvider ?? throw new ArgumentNullException(nameof(feedMapperProvider));
            _feedProcessingManager = feedProcessingManager ?? throw new ArgumentNullException(nameof(feedProcessingManager));
            _logger = loggerFactory.CreateLogger<FeedValidator>();
        }

        [Function(FunctionNames.FeedValidator)]
        public async Task Run(
            [QueueTrigger(StorageQueues.Validate, Connection = ConnectionStrings.AzureStorageAccount)] FeedBlob feedBlob,
            FunctionContext context)
        {
            _logger.LogInformation($"Validator | Validating FeedBlob - CustomerId: {feedBlob.CustomerId}");

            try
            {
                var customerId = Guid.Parse(feedBlob.CustomerId);

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

                // Download blob and process feed data
                var feedDataFormat = await GetFeedDataFormat(customerId, feedBlob, _logger);
                var feedMapper = _feedMapperProvider.GetMapper(feedDataFormat);
                var feedData = await feedMapper.GetDataAsync(csvBlobClient);

                var hasFeedData = HasFeedData(feedData, _logger);
                if (hasFeedData)
                {
                    await _feedProcessingManager.MoveCsvBlobAsync(csvBlobClient, feedBlob);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Validator | Error: {ex.Message}");
            }
        }

        private bool HasFeedData<T>(IEnumerable<T> result, ILogger logger)
        {
            var feedDataCount = result?.Count() ?? 0;

            logger.LogInformation($"Validator | Data count: {feedDataCount}");

            if (feedDataCount == 0)
            {
                logger.LogWarning("Validator | Failed to validate");
                return false;
            }

            logger.LogInformation("Validator | Successfully Validated");
            return true;
        }

        /// <summary>
        /// Returns the feed data format for the customerId & search index name
        /// If null, logs a critical error and throws an exception
        /// </summary>
        private async Task<string> GetFeedDataFormat(Guid customerId, FeedBlob feedBlob, ILogger logger)
        {
            var feedDataFormat = await _feedRepo.GetDataFormatAsync(customerId, feedBlob.SearchIndexName);

            if (string.IsNullOrEmpty(feedDataFormat))
            {
                // Log detailed information and throw an exception
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"CustomerId:{customerId}");
                sb.AppendLine($"FeedBlob customerId:{feedBlob.CustomerId}");
                sb.AppendLine($"SearchIndexName:{feedBlob.SearchIndexName}");
                sb.AppendLine($"BlobUri:{feedBlob.BlobUri}");
                sb.AppendLine($"FileName:{feedBlob.FileName}");
                sb.AppendLine($"CurrentFeedArea:{feedBlob.CurrentFeedArea}");

                var message = $"GetFeedDataFormat returned null | Error: {sb.ToString()}";
                logger.LogError(message);
                throw new FeedDataFormatException(message);
            }

            return feedDataFormat;
        }
    }
}
