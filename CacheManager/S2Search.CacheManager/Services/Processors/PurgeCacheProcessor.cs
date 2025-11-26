using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Models;
using Services.Interfaces.Managers;
using Services.Interfaces.Processors;
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

            TimeSpan backoff = TimeSpan.FromSeconds(2);
            TimeSpan maxBackoff = TimeSpan.FromSeconds(30);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var messages = await queueManager.GetMessagesAsync(StorageQueues.PurgeCache, cancellationToken);

                        if (messages == null || messages.Length == 0)
                        {
                            // No work found — wait with exponential backoff
                            logger.LogInformation("No messages found. Backing off for {BackoffSeconds}s.", backoff.TotalSeconds);
                            await Task.Delay(backoff, cancellationToken);
                            backoff = TimeSpan.FromSeconds(Math.Min(maxBackoff.TotalSeconds, backoff.TotalSeconds * 2));
                            continue;
                        }

                        // Reset backoff when work is found
                        backoff = TimeSpan.FromSeconds(2);

                        logger.LogInformation("Messages to process: {Count}", messages.Length);

                        foreach (var message in messages)
                        {
                            // stop quickly if cancellation requested
                            cancellationToken.ThrowIfCancellationRequested();
                            await ProcessMessageAsync(message, cancellationToken);
                        }
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        // graceful shutdown requested
                        logger.LogInformation("Cancellation requested. Exiting polling loop.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        // transient error — log and continue after a short delay
                        logger.LogError(ex, "Unhandled exception while polling/processing messages. Retrying after delay.");
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    }
                }
            }
            finally
            {
                logger.LogInformation("PurgeCacheProcessor stopped.");
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
                var hostCacheKey = Generate(purgeCacheMessage.Host);

                logger.LogInformation("Deleting Cache for {HostCacheKey}", hostCacheKey);

                await cacheManager.DeleteKeysByWildcard(hostCacheKey);

                logger.LogInformation("Deleting MessageId: {MessageId}", message.MessageId);

                await queueManager.DeleteMessageAsync(StorageQueues.PurgeCache, message, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on {Method} | Message: {MessageId} | DecodedMessage: {DecodedMessage}", nameof(ProcessMessageAsync), message.MessageId, decodedMessage);
            }
        }

        private static string Generate(string host)
        {
            var formattedHost = host.Replace(":", string.Empty);
            return $"{formattedHost}:";
        }
    }
}
