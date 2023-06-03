using Azure.Storage.Queues;

namespace Services.Interfaces.Providers
{
    public interface IQueueClientProvider
    {
        Task<bool> TestConnectionAsync(string connectionKey, string queueName);
        Task<QueueClient> GetAsync(string connectionKey, string queueName);
    }
}
