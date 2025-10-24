using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Repositories
{
    public class SearchIndexCredentialsRepository : ISearchIndexCredentialsRepository
    {
        private readonly string _connectionstring;
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;

        public SearchIndexCredentialsRepository(IConfiguration configuration, IDbContextProvider dbContext, ILogger<SearchIndexCredentialsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionstring = configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase) ?? throw new InvalidOperationException($"{ConnectionStringKeys.SqlDatabase} connection string not found.");
        }

        public async Task<SearchIndexCredentials> GetCredentials(Guid customerId, string searchIndexName)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "customer_id", customerId },
                    { "search_index_name", searchIndexName }
                };

                var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexCredentials>(_connectionstring,
                                                                                StoredProcedures.GetSearchIndexCredentials,
                                                                                parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCredentials)} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
