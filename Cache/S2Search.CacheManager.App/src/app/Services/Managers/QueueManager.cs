using Azure.Storage.Queues.Models;
using Domain.Constants;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;

namespace Services.Managers
{
    internal class QueueManager : IQueueManager
    {
        private readonly IQueueClientProvider _queueClient;

        public QueueManager(IQueueClientProvider queueClient)
        {
            _queueClient = queueClient;
        }

        public Task<bool> TestConnectionAsync(string queueName, CancellationToken cancellationToken = default)
        {
            var canConnect = _queueClient.TestConnectionAsync(ConnectionStrings.StorageQueue, queueName);
            return canConnect;
        }

        public async Task<QueueMessage[]> GetMessagesAsync(string queueName, CancellationToken cancellationToken = default)
        {
            var client = await _queueClient.GetAsync(ConnectionStrings.StorageQueue, queueName);
            var messages = await client.ReceiveMessagesAsync(maxMessages: 25, cancellationToken: cancellationToken);

            return messages;
        }

        public async Task DeleteMessageAsync(string queueName, QueueMessage message, CancellationToken cancellationToken = default)
        {
            var client = await _queueClient.GetAsync(ConnectionStrings.StorageQueue, queueName);

            _ = await client.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
        }
    }
}
