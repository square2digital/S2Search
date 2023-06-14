using Domain.SearchResources.Feeds;

namespace Services.Interfaces.Managers
{
    public interface IFeedSettingsValidationManager
    {
        bool IsValid(FeedRequest feedRequest, out string errorMessage);
    }
}
