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
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;

        public SearchIndexCredentialsRepository(IDbContextProvider dbContext,
                                                ILogger<SearchIndexCredentialsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SearchIndexCredentials> GetCredentials(Guid customerId, string searchIndexName)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "CustomerId", customerId },
                    { "SearchIndexName", searchIndexName}
                };

                var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexCredentials>(ConnectionStrings.SqlDatabase,
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
