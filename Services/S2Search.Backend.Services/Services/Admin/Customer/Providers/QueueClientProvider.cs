using Azure.Storage.Queues;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
{
    public class QueueClientProvider : IQueueClientProvider
    {
        private readonly IAppSettings _appSettings;

        public QueueClientProvider(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<QueueClient> GetAsync(string connectionString, string queueName)
        {
            var queueClient = CreateQueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }

        public async Task<bool> TestConnectionAsync(string connectionKey, string queueName)
        {
            var queueClient = CreateQueueClient(_appSettings.ConnectionStrings.Redis, queueName);
            try
            {
                await queueClient.CreateIfNotExistsAsync();
                var response = await queueClient.PeekMessageAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private QueueClient CreateQueueClient(string connectionString, string queueName)
        {
            return new QueueClient(connectionString, queueName);
        }
    }
}
