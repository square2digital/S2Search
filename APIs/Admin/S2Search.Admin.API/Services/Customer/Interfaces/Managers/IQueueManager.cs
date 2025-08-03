using Domain.Customer.Messages;

namespace Services.Customer.Interfaces.Managers
{
    public interface IQueueManager
    {
        Task EnqueueMessageAsync(QueuedMessage message);
        QueuedMessage CreateMessage(string targetQueue, object messageData);
    }
}
