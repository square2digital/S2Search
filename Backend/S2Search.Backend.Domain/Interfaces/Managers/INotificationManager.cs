using S2Search.Backend.Domain.Customer.SearchResources.Notifications;
using S2Search.Backend.Domain.Customer.Shared;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface INotificationManager
{
    Task<PagedResults<Notification>> GetNotificationsPaged(Guid searchIndexId, int rowsToSkip, int pageSize);
}
