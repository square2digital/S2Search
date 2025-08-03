using Domain.Messages;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface IQueueManager
    {
        Task EnqueueMessageAsync(QueuedMessage message);
        QueuedMessage CreateMessage(string targetQueue, object messageData);
    }
}
