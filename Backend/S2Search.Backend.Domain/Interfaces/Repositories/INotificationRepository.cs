using S2Search.Backend.Domain.Customer.SearchResources.Notifications;
using S2Search.Backend.Domain.Customer.Shared;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<PagedResults<Notification>> GetNotificationsPagedAsync(Guid searchIndexId, DateTime startDate, DateTime endDate, int rowsToSkip, int pageSize);
    Task AddNotificationAsync(Notification notification);
}
