namespace Services.Interfaces.Managers
{
    public interface IFeedTargetQueueManager
    {
        string GetTargetQueue(string currentFeedArea);
    }
}
