using Azure.Storage.Queues.Models;
using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using Services.Interfaces.Processors;
using Services.Managers;
using System.Text;
using System.Text.Json;

namespace Services.Processors
{
    internal class PurgeCacheProcessor : IPurgeCacheProcessor
    {
        private readonly ILogger logger;
        private readonly IQueueManager queueManager;
        private readonly ICacheManager cacheManager;

        public PurgeCacheProcessor(ILogger<PurgeCacheProcessor> logger,
                                   IQueueManager queueManager,
                                   ICacheManager cacheManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.queueManager = queueManager ?? throw new ArgumentNullException(nameof(queueManager));
            this.cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting PurgeCacheProcessor...");

            var canConnectToStorageAccount = await queueManager.TestConnectionAsync(StorageQueues.PurgeCache, cancellationToken);

            if (!canConnectToStorageAccount)
            {
                var message = $"Unable to connect to Storage Account using this Configuration Key: '{ConnectionStrings.StorageQueue}'";
                var exception = new Exception(message);
                logger.LogCritical(exception, message);
                return;
            }

            try
            {
                var messages = await queueManager.GetMessagesAsync(StorageQueues.PurgeCache, cancellationToken);

                logger.LogInformation("Messages to process: {messages.Length}", messages.Length);

                foreach (var message in messages)
                {
                    await ProcessMessageAsync(message, cancellationToken);
                }

                logger.LogInformation("Shutting down PurgeCacheProcessor...");
            }
            catch (Exception ex)
            {
                var message = $"Exception on {nameof(RunAsync)} - Message: {ex.Message}";
                logger.LogError(ex, message);
            }
        }

        private async Task ProcessMessageAsync(QueueMessage message, CancellationToken cancellationToken)
        {
            var messageBytes = Convert.FromBase64String(message.Body.ToString());
            var decodedMessage = Encoding.UTF8.GetString(messageBytes);

            try
            {
                var purgeCacheMessage = JsonSerializer.Deserialize<PurgeCacheMessage>(decodedMessage);
                var hostCacheKey = S2SearchCacheKeyGenerationManager.Generate(purgeCacheMessage.Host);

                logger.LogInformation($"Deleting Cache for {hostCacheKey}");

                await cacheManager.DeleteKeysByWildcard(hostCacheKey);

                logger.LogInformation($"Deleting MessageId: {message.MessageId}");

                await queueManager.DeleteMessageAsync(StorageQueues.PurgeCache, message, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception on {nameof(ProcessMessageAsync)} | Message: {message.MessageId} | DecodedMessage: {decodedMessage}");
            }
        }
    }
}
