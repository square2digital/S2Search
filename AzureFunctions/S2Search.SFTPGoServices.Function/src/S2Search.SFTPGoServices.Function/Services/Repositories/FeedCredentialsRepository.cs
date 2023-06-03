using Domain.Constants;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class FeedCredentialsRepository : IFeedCredentialsRepository
    {
        private readonly IDbContextProvider _dbContext;

        public FeedCredentialsRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Guid searchIndexId, string username, string passwordHash)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "Username", username},
                { "PasswordHash", passwordHash}
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.AddFeedCredentials, parameters);
        }

        public async Task Delete(Guid searchIndexId, string username)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "Username", username}
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.DeleteFeedCredentials, parameters);
        }

        public async Task Update(Guid searchIndexId, string username, string passwordHash)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "Username", username},
                { "PasswordHash", passwordHash}
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.UpdateFeedCredentials, parameters);
        }
    }
}
