using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAzureQueueClientProvider
    {
        Task<QueueClient> GetAsync(string connectionName, string queueName);
    }
}
