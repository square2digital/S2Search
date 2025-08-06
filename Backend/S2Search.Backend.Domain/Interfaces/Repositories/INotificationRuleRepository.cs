using S2Search.Backend.Domain.Customer.SearchResources.NotificationRules;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories
{
    public interface INotificationRuleRepository
    {
        Task<NotificationRule> CreateAsync(NotificationRuleRequest notificationRuleRequest);
        Task DeleteAsync(Guid searchIndexId, int notificationRuleId);
        Task<NotificationRule> GetByIdAsync(Guid searchIndexId, int notificationRuleId);
        Task<IEnumerable<NotificationRule>> GetByIdAsync(Guid searchIndexId);
    }
}
