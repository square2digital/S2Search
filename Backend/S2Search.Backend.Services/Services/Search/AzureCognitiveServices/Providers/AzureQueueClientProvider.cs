using Azure.Storage.Queues;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers
{
    public class AzureQueueClientProvider : IAzureQueueClientProvider
    {
        private readonly IConnectionStringProvider _connectionString;

        public AzureQueueClientProvider(IConnectionStringProvider connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<QueueClient> GetAsync(string connectionName, string queueName)
        {
            var queueClient = CreateQueueClient(_connectionString.Get(connectionName), queueName);
            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }

        private QueueClient CreateQueueClient(string connectionString, string queueName)
        {
            return new QueueClient(connectionString, queueName);
        }
    }
}
