﻿using Domain.Customer.Constants;
using Domain.Customer.Messages;
using Newtonsoft.Json;
using Services.Customer.Interfaces.Managers;
using Services.Customer.Interfaces.Providers;
using System.Text;

namespace Services.Customer.Managers
{
    public class QueueManager : IQueueManager
    {
        private readonly IQueueClientProvider _queueClient;

        public QueueManager(IQueueClientProvider queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task EnqueueMessageAsync(QueuedMessage message)
        {
            string _message = ConvertObjectToBase64String(message.Data);
            var client = await _queueClient.GetAsync(ConnectionStrings.StorageQueue, message.TargetQueue);
            await client.SendMessageAsync(_message);
        }

        private static string ConvertObjectToBase64String(object message)
        {
            string messageAsString;
            if (message is string || message is Guid)
            {
                messageAsString = message.ToString();
            }
            else
            {
                messageAsString = JsonConvert.SerializeObject(message, Formatting.Indented);
            }

            var messageBytes = Encoding.UTF8.GetBytes(messageAsString);
            return Convert.ToBase64String(messageBytes);
        }

        public QueuedMessage CreateMessage(string targetQueue, object messageData)
        {
            return new QueuedMessage(targetQueue, messageData);
        }
    }
}
