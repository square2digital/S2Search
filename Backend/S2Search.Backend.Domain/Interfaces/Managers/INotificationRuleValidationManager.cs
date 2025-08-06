using S2Search.Backend.Domain.Customer.SearchResources.NotificationRules;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers
{
    public interface INotificationRuleValidationManager
    {
        bool IsValid(NotificationRuleRequest notificationRuleRequest, out string errorMessage);
    }
}
