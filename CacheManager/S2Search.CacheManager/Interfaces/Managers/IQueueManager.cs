using Azure.Storage.Queues.Models;

namespace S2Search.CacheManager.Interfaces.Managers
{
    public interface IQueueManager
    {
        Task<bool> TestConnectionAsync(string queueName, CancellationToken cancellationToken = default);
        Task<QueueMessage[]> GetMessagesAsync(string queueName, CancellationToken cancellationToken = default);
        Task DeleteMessageAsync(string queueName, QueueMessage message, CancellationToken cancellationToken = default);
    }
}
