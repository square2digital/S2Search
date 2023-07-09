using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace Services.Customer.Interfaces.Providers
{
    public interface IQueueClientProvider
    {
        Task<QueueClient> GetAsync(string connectionKey, string queueName);
    }
}
