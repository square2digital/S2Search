using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Exceptions;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace S2Search.Function.FeedServices;

public class FeedValidator
{
    private readonly IFeedServicesRepository _feedRepo;
    private readonly IFeedMapperProvider _feedMapperProvider;
    private readonly IFeedProcessingManager _feedProcessingManager;
    private readonly ILogger<FeedValidator> _logger;

    public FeedValidator(IFeedServicesRepository feedRepo,
                         IFeedMapperProvider feedMapperProvider,
                         IFeedProcessingManager feedProcessingManager,
                         ILogger<FeedValidator> logger)
    {
        _feedRepo = feedRepo;
        _feedMapperProvider = feedMapperProvider;
        _feedProcessingManager = feedProcessingManager;
        _logger = logger;
    }

    [Function(nameof(FeedValidator))]
    public async Task Run([QueueTrigger(StorageQueues.Validate, Connection = ConnectionStrings.AzureStorageAccount)]
        FeedBlob feedBlob,
        IBinder binder,
        ILogger logger)
    {
        logger.LogInformation($"Validator | Validating FeedBlob - CustomerId: {feedBlob.CustomerId}");

        try
        {
            var customerId = Guid.Parse(feedBlob.CustomerId);

            // Use modern Azure SDK BlobClient constructed from the provided blob URI
            var csvBlobClient = new BlobClient(new Uri(feedBlob.BlobUri));

            var feedDataFormat = await GetFeedDataFormat(customerId, feedBlob, logger);
            var feedMapper = _feedMapperProvider.GetMapper(feedDataFormat);
            var feedData = await feedMapper.GetDataAsync(csvBlobClient);

            var hasFeedData = HasFeedData(feedData, logger);
            if (hasFeedData)
            {
                await _feedProcessingManager.MoveCsvBlobAsync(csvBlobClient, feedBlob);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Validator | Error: {ex.Message}");
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
    /// returns the feed data format for the customerId & searchindex name
    /// if its null it will logs a critical error and throw an exception
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="feedBlob"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    private async Task<string> GetFeedDataFormat(Guid customerId, FeedBlob feedBlob, ILogger logger)
    {
        var feedDataFormat = await _feedRepo.GetDataFormatAsync(customerId, feedBlob.SearchIndexName);

        if (string.IsNullOrEmpty(feedDataFormat))
        {
            // if the feedDataFormat is null it means that there is a config issue for the customer
            // start by investigating the "KeyPrefix" in SFTPgo for this user

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