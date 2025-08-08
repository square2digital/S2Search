using S2Search.Backend.Domain.Models;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

public interface IAzureQueueService
{
    Task EnqueueMessageAsync(QueuedMessage message);
    QueuedMessage CreateMessage(string targetQueue, object messageData);
}
