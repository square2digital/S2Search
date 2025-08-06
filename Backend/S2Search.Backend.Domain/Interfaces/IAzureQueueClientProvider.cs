using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces
{
    public interface IAzureQueueClientProvider
    {
        Task<QueueClient> GetAsync(string connectionName, string queueName);
    }
}
