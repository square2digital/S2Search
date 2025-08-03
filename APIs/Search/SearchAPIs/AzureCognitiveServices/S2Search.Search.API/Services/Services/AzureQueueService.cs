using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly IAzureQueueClientProvider _azureQueueClient;
        private readonly ILogger<AzureQueueService> _logger;

        public AzureQueueService(IAzureQueueClientProvider azureQueueClient,
                                 ILogger<AzureQueueService> logger)
        {
            _azureQueueClient = azureQueueClient ?? throw new ArgumentNullException(nameof(azureQueueClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task EnqueueMessageAsync(QueuedMessage message)
        {
            try
            {
                string encodedMessage = ConvertObjectToBase64String(message.Data);
                var client = await _azureQueueClient.GetAsync(ConnectionStrings.StorageQueue, message.TargetQueue);
                await client.SendMessageAsync(encodedMessage);
            }
            catch(Exception ex)
            {
                //log the error and dont throw as to not impact search queries
                _logger.LogError($"Exception on {nameof(EnqueueMessageAsync)} | Timestamp: {DateTime.Now.ToString("MM/dd/yyyy h:mm tt")} | Message: {ex.Message}");
            }
        }

        private static string ConvertObjectToBase64String(object message)
        {
            string messageAsString;
            if (message is string || message is Guid)
            {
                messageAsString = message.ToString();
            }
            else
            {
                messageAsString = JsonConvert.SerializeObject(message, Formatting.Indented);
            }

            var messageBytes = Encoding.UTF8.GetBytes(messageAsString);
            return Convert.ToBase64String(messageBytes);
        }

        public QueuedMessage CreateMessage(string targetQueue, object messageData)
        {
            return new QueuedMessage(targetQueue, messageData);
        }
    }
}
