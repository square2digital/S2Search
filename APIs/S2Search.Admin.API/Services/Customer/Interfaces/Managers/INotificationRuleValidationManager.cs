using Domain.Customer.SearchResources.NotificationRules;

namespace Services.Customer.Interfaces.Managers
{
    public interface INotificationRuleValidationManager
    {
        bool IsValid(NotificationRuleRequest notificationRuleRequest, out string errorMessage);
    }
}
