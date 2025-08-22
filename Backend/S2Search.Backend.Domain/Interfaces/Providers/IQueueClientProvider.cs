using Azure.Storage.Queues;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

public interface IQueueClientProvider
{
    Task<bool> TestConnectionAsync(string connectionKey, string queueName);
    Task<QueueClient> GetAsync(string connectionKey, string queueName);
}