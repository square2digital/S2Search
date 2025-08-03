using Domain.SearchResources.Feeds;
using Services.Interfaces.Managers;
using System;

namespace Services.Managers
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
