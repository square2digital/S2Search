using Azure.Storage.Queues;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
{
    public class QueueClientProvider : IQueueClientProvider
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
