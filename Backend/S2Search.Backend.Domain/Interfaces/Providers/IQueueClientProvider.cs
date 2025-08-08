using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

public interface IQueueClientProvider
{
    Task<QueueClient> GetAsync(string connectionKey, string queueName);
}
