using Azure.Storage.Queues;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Providers;

namespace Services.Providers
{
    internal class QueueClientProvider : IQueueClientProvider
    {
        private readonly IConnectionStringProvider _connectionString;

        public QueueClientProvider(IConnectionStringProvider connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> TestConnectionAsync(string connectionKey, string queueName)
        {
            var queueClient = CreateQueueClient(_connectionString.Get(connectionKey), queueName);
            try
            {
                var response = await queueClient.PeekMessageAsync();
                return true;
            }
            catch
            {
                return false;
            }
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
