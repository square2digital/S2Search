using Domain.Constants;
using Domain.SearchResources.Notifications;
using Domain.Shared;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbContextProvider _dbContext;

        public NotificationRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", notification.SearchIndexId },
                { "Recipients", notification.Recipients },
                { "Event", notification.Event },
                { "Category", notification.Category },
                { "TransmitType", notification.TransmitType }
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.AddNotification, parameters);
        }

        public async Task<PagedResults<Notification>> GetNotificationsPagedAsync(Guid searchIndexId, DateTime startDate, DateTime endDate, int rowsToSkip, int pageSize)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "StartDate", startDate },
                { "EndDate", endDate },
                { "RowsToSkip", rowsToSkip },
                { "PageSize", pageSize },
            };

            var results = await _dbContext.QueryMultipleAsync<PagedResults<Notification>>(ConnectionStrings.CustomerResourceStore,
                                                                                          StoredProcedures.GetNotificationsForSearchIndex,
                                                                                          parameters);

            return results;
        }
    }
}
