using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace S2Search.FeedServices.Function
{
    public class FeedValidator
    {
        private readonly IFeedRepository _feedRepo;
        private readonly IFeedMapperProvider _feedMapperProvider;
        private readonly IFeedProcessingManager _feedProcessingManager;

        public FeedValidator(IFeedRepository feedRepo,
                             IFeedMapperProvider feedMapperProvider,
                             IFeedProcessingManager feedProcessingManager)
        {
            _feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
            _feedMapperProvider = feedMapperProvider ?? throw new ArgumentNullException(nameof(feedMapperProvider));
            _feedProcessingManager = feedProcessingManager ?? throw new ArgumentNullException(nameof(feedProcessingManager));
        }

        [FunctionName(FunctionNames.FeedValidator)]
        public async Task Run([QueueTrigger(StorageQueues.Validate, Connection = ConnectionStrings.AzureStorageAccount)] FeedBlob feedBlob,
                                IBinder binder,
                                ILogger logger)
        {
            logger.LogInformation($"Validator | Validating FeedBlob - CustomerId: {feedBlob.CustomerId}");

            try
            {
                var customerId = Guid.Parse(feedBlob.CustomerId);
                var csvBlob = await binder.BindAsync<CloudBlockBlob>(new BlobAttribute(feedBlob.BlobUri));
                var feedDataFormat = await GetFeedDataFormat(customerId, feedBlob, logger);
                var feedMapper = _feedMapperProvider.GetMapper(feedDataFormat);
                var feedData = await feedMapper.GetDataAsync(csvBlob);

                var hasFeedData = HasFeedData(feedData, logger);
                if (hasFeedData)
                {
                    await _feedProcessingManager.MoveCsvBlobAsync(csvBlob, feedBlob);
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
}
