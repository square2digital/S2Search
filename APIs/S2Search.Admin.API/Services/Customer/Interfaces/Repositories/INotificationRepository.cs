using Domain.Customer.SearchResources.Notifications;
using Domain.Customer.Shared;

namespace Services.Customer.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<PagedResults<Notification>> GetNotificationsPagedAsync(Guid searchIndexId, DateTime startDate, DateTime endDate, int rowsToSkip, int pageSize);
        Task AddNotificationAsync(Notification notification);
    }
}
