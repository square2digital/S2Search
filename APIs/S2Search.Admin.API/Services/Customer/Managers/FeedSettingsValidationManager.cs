using Domain.Customer.Enums;
using Domain.Customer.SearchResources.Feeds;
using Services.Customer.Interfaces.Managers;

namespace Services.Customer.Managers
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

            if(feedRequest.ScheduleMinutes <= 0 || (feedRequest.ScheduleMinutes % 5) != 0)
            {
                errorMessage = $"Invalid {nameof(feedRequest.ScheduleMinutes)}";
                return false;
            }

            return true;
        }
    }
}
