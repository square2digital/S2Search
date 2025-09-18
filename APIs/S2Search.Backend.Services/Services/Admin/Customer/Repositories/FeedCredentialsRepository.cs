using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class FeedCredentialsRepository : IFeedCredentialsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextProvider _dbContext;
        private readonly string _connectionString;

        public FeedCredentialsRepository(IConfiguration configuration, IDbContextProvider dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = configuration.GetConnectionString("S2_Search");
        }

        public async Task<bool> CheckUserExists(Guid searchIndexId, string username)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "Username", username }
            };

            var credentials = await _dbContext.QuerySingleOrDefaultAsync<FeedCredentials>(_connectionString,
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

            var credentials = await _dbContext.QuerySingleOrDefaultAsync<FeedCredentials>(_connectionString,
                                                                                    StoredProcedures.GetFeedCredentialsUsername,
                                                                                    parameters);
            
            return credentials;
        }
    }
}
