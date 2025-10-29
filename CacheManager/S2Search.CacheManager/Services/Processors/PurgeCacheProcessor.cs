using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Models;
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
        private readonly string _connectionString;

        public PurgeCacheProcessor(IConfiguration configuration,
                                   ILogger<PurgeCacheProcessor> logger,
                                   IQueueManager queueManager,
                                   ICacheManager cacheManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.queueManager = queueManager ?? throw new ArgumentNullException(nameof(queueManager));
            this.cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _connectionString = configuration.GetConnectionString(ConnectionStringKeys.AzureStorage);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting PurgeCacheProcessor...");

            var canConnectToStorageAccount = await queueManager.TestConnectionAsync(StorageQueues.PurgeCache, cancellationToken);

            if (!canConnectToStorageAccount)
            {
                var message = $"Unable to connect to Storage Account using this Configuration String: '{_connectionString}'";
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
                var jsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                var purgeCacheMessage = JsonSerializer.Deserialize<PurgeCacheMessage>(decodedMessage, jsonSerializerOptions);
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
