using Domain.Customer.SearchResources.Feeds;

namespace Services.Customer.Interfaces.Managers
{
    public interface IFeedSettingsValidationManager
    {
        bool IsValid(FeedRequest feedRequest, out string errorMessage);
    }
}
