using Domain.Constants;
using Services.Interfaces.Managers;

namespace Services.Managers
{
    public class FeedTargetQueueManager : IFeedTargetQueueManager
    {
        public string GetTargetQueue(string currentFeedArea)
        {
            string targetQueue = currentFeedArea switch
            {
                FeedAreas.Extract => StorageQueues.Extract,
                FeedAreas.Validate => StorageQueues.Validate,
                FeedAreas.Process => StorageQueues.Process,
                _ => "unknown",
            };

            return targetQueue;
        }
    }
}
