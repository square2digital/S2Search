using Domain.SearchResources.Notifications;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<PagedResults<Notification>> GetNotificationsPagedAsync(Guid searchIndexId, DateTime startDate, DateTime endDate, int rowsToSkip, int pageSize);
        Task AddNotificationAsync(Notification notification);
    }
}
