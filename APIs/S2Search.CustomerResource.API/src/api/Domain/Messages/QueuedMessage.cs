namespace Domain.Messages
{
    public class QueuedMessage
    {
        public string TargetQueue { get; }
        public object Data { get; }

        public QueuedMessage(string targetQueue, object data)
        {
            TargetQueue = targetQueue;
            Data = data;
        }
    }
}
