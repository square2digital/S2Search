using Domain.SearchResources.Notifications;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface INotificationManager
    {
        Task<PagedResults<Notification>> GetNotificationsPaged(Guid searchIndexId, int rowsToSkip, int pageSize);
    }
}
