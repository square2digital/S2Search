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
            var queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }

        public async Task<bool> TestConnectionAsync(string connectionString, string queueName)
        {
            try
            {
                var queueClient = new QueueClient(connectionString, queueName);

                await queueClient.CreateIfNotExistsAsync();
                await queueClient.PeekMessageAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
