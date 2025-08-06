using S2Search.Backend.Domain.Customer.Enums;
using S2Search.Backend.Domain.Customer.SearchResources.Feeds;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class FeedSettingsValidationManager : IFeedSettingsValidationManager
    {
        public bool IsValid(FeedRequest feedRequest, out string errorMessage)
        {
            errorMessage = "";
            if (!Enum.TryParse(feedRequest.FeedType, out FeedType _))
            {
                errorMessage = $"Invalid {nameof(feedRequest.FeedType)}";
                return false;
            }

            if(feedRequest.ScheduleMinutes <= 0 || feedRequest.ScheduleMinutes % 5 != 0)
            {
                errorMessage = $"Invalid {nameof(feedRequest.ScheduleMinutes)}";
                return false;
            }

            return true;
        }
    }
}
