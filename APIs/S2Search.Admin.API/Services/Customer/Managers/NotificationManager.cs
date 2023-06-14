using Domain.SearchResources.Notifications;
using Domain.Shared;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Services.Managers
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
