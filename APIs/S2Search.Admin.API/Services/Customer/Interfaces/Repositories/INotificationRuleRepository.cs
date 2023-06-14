using Domain.SearchResources.NotificationRules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface INotificationRuleRepository
    {
        Task<NotificationRule> CreateAsync(NotificationRuleRequest notificationRuleRequest);
        Task DeleteAsync(Guid searchIndexId, int notificationRuleId);
        Task<NotificationRule> GetByIdAsync(Guid searchIndexId, int notificationRuleId);
        Task<IEnumerable<NotificationRule>> GetByIdAsync(Guid searchIndexId);
    }
}
