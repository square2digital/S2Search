using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class FeedCredentialsRepository : IFeedCredentialsRepository
    {
        private readonly IDbContextProvider _dbContext;
        public FeedCredentialsRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckUserExists(Guid searchIndexId, string username)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "Username", username }
            };

            var credentials = await _dbContext.QuerySingleOrDefaultAsync<FeedCredentials>(ConnectionStrings.CustomerResourceStore,
                                                                                    StoredProcedures.GetFeedCredentials,
                                                                                    parameters);
            bool userExists = credentials != null && credentials.SearchIndexId != Guid.Empty;
            return userExists;
        }

        public async Task<FeedCredentials> GetCredentials(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var credentials = await _dbContext.QuerySingleOrDefaultAsync<FeedCredentials>(ConnectionStrings.CustomerResourceStore,
                                                                                    StoredProcedures.GetFeedCredentialsUsername,
                                                                                    parameters);
            
            return credentials;
        }
    }
}
