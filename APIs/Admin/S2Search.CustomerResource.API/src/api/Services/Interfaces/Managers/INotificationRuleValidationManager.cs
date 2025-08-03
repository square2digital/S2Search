using Domain.SearchResources.NotificationRules;

namespace Services.Interfaces.Managers
{
    public interface INotificationRuleValidationManager
    {
        bool IsValid(NotificationRuleRequest notificationRuleRequest, out string errorMessage);
    }
}
