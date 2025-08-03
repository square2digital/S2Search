using Domain.Customer.SearchResources.Notifications;
using Domain.Customer.Shared;

namespace Services.Customer.Interfaces.Managers
{
    public interface INotificationManager
    {
        Task<PagedResults<Notification>> GetNotificationsPaged(Guid searchIndexId, int rowsToSkip, int pageSize);
    }
}
