using Azure.Storage.Queues;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
{
    public class QueueClientProvider : IQueueClientProvider
    {
        private readonly IConnectionStringProvider _connectionString;

        public QueueClientProvider(IConnectionStringProvider connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<QueueClient> GetAsync(string connectionKey, string queueName)
        {
            var queueClient = CreateQueueClient(_connectionString.Get(connectionKey), queueName);
            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }

        private QueueClient CreateQueueClient(string connectionString, string queueName)
        {
            return new QueueClient(connectionString, queueName);
        }
    }
}
