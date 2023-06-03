using Domain.Enums;
using Domain.SearchResources.NotificationRules;
using Services.Interfaces.Managers;
using System;

namespace Services.Managers
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
