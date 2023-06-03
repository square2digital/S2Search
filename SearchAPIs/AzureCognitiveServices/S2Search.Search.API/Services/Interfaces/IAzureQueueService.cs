using Domain.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAzureQueueService
    {
        Task EnqueueMessageAsync(QueuedMessage message);
        QueuedMessage CreateMessage(string targetQueue, object messageData);
    }
}
