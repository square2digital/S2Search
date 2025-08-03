using Domain.Customer.SearchResources.NotificationRules;

namespace Services.Customer.Interfaces.Repositories
{
    public interface INotificationRuleRepository
    {
        Task<NotificationRule> CreateAsync(NotificationRuleRequest notificationRuleRequest);
        Task DeleteAsync(Guid searchIndexId, int notificationRuleId);
        Task<NotificationRule> GetByIdAsync(Guid searchIndexId, int notificationRuleId);
        Task<IEnumerable<NotificationRule>> GetByIdAsync(Guid searchIndexId);
    }
}
