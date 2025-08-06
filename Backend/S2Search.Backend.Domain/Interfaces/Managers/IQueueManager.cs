using S2Search.Backend.Domain.Customer.Messages;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers
{
    public interface IQueueManager
    {
        Task EnqueueMessageAsync(QueuedMessage message);
        QueuedMessage CreateMessage(string targetQueue, object messageData);
    }
}
