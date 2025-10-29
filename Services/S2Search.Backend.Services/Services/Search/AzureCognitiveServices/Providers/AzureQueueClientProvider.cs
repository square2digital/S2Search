using Azure.Storage.Queues;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers
{
    public class AzureQueueClientProvider : IAzureQueueClientProvider
    {
        public async Task<QueueClient> GetAsync(string connectionString, string queueName)
        {
            var queueClient = CreateQueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }

        private QueueClient CreateQueueClient(string connectionString, string queueName)
        {
            return new QueueClient(connectionString, queueName);
        }
    }
}
