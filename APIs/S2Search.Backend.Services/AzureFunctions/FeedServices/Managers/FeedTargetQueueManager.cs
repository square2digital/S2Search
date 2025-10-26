using S2Search.Backend.Domain.AzureFunctions.FeedServices.Constants;
using S2Search.Backend.Domain.Constants;
using Services.Interfaces.Managers;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Managers
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
