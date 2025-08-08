using S2Search.Backend.Domain.Customer.SearchResources.Notifications;
using S2Search.Backend.Domain.Customer.Shared;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int _maxNumberOfDays = 30;

        public NotificationManager(INotificationRepository notificationRepo,
                                   IDateTimeProvider dateTimeProvider)
        {
            _notificationRepo = notificationRepo;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<PagedResults<Notification>> GetNotificationsPaged(Guid searchIndexId, int rowsToSkip, int pageSize)
        {
            var endDate = _dateTimeProvider.CurrentDateTime();
            var startDate = endDate.AddDays(-_maxNumberOfDays);

            var results = _notificationRepo.GetNotificationsPagedAsync(searchIndexId, startDate, endDate, rowsToSkip, pageSize);

            return results;
        }
    }
}
