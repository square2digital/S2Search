﻿using Domain.Constants;
using Domain.SearchResources.NotificationRules;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Managers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class NotificationRuleRepository : INotificationRuleRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly INotificationRuleValidationManager _notificationRuleValidation;

        public NotificationRuleRepository(IDbContextProvider dbContext,
                                      INotificationRuleValidationManager notificationRuleValidation)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _notificationRuleValidation = notificationRuleValidation ?? throw new ArgumentNullException(nameof(notificationRuleValidation));
        }

        public async Task<NotificationRule> CreateAsync(NotificationRuleRequest notificationRuleRequest)
        {
            if (!_notificationRuleValidation.IsValid(notificationRuleRequest, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", notificationRuleRequest.SearchIndexId },
                { "TransmitType", notificationRuleRequest.TransmitType },
                { "Recipients", notificationRuleRequest.Recipients },
                { "Trigger", notificationRuleRequest.TriggerType },
            };

            var notificationRuleId = await _dbContext.ExecuteScalarAsync<int>(ConnectionStrings.CustomerResourceStore,
                                                                          StoredProcedures.AddNotificationRule,
                                                                          parameters);

            var result = await GetByIdAsync(notificationRuleRequest.SearchIndexId, notificationRuleId);
            return result;
        }

        public async Task DeleteAsync(Guid SearchIndexId, int notificationRuleId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId },
                { "NotificationRuleId", notificationRuleId }
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.SupersedeNotificationRule, parameters);
        }

        public async Task<NotificationRule> GetByIdAsync(Guid SearchIndexId, int notificationRuleId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId },
                { "NotificationRuleId", notificationRuleId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<NotificationRule>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetNotificationRuleById, parameters);

            return result;
        }

        public async Task<IEnumerable<NotificationRule>> GetByIdAsync(Guid SearchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId }
            };

            var results = await _dbContext.QueryAsync<NotificationRule>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetNotificationRules, parameters);
            
            return results;
        }
    }
}
