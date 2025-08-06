using S2Search.Backend.Domain.Customer.Enums;
using S2Search.Backend.Domain.Customer.SearchResources.NotificationRules;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class NotificationRuleValidationManager : INotificationRuleValidationManager
    {
        public bool IsValid(NotificationRuleRequest notificationRuleRequest, out string errorMessage)
        {
            errorMessage = "";
            if (!Enum.TryParse(notificationRuleRequest.TransmitType, out NotificationTransmitType _))
            {
                errorMessage += $"Invalid {nameof(notificationRuleRequest.TransmitType)}{Environment.NewLine}";
            }

            if (!Enum.TryParse(notificationRuleRequest.TriggerType, out NotificationTriggerType _))
            {
                errorMessage += $"Invalid {nameof(notificationRuleRequest.TriggerType)}";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return false;
            }

            return true;
        }
    }
}
