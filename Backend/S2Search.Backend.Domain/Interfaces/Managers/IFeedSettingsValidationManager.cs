using S2Search.Backend.Domain.Customer.SearchResources.Feeds;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface IFeedSettingsValidationManager
{
    bool IsValid(FeedRequest feedRequest, out string errorMessage);
}
