using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Interfaces.Providers;
using Services.Interfaces.Repositories;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Repositories
{
    public class GenericSynonymRepository : IGenericSynonymRepository
    {
        private readonly string _connectionstring;
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;

        public GenericSynonymRepository(IConfiguration configuration, IDbContextProvider dbContext, ILogger<GenericSynonymRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionstring = configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase) ?? throw new InvalidOperationException($"{ConnectionStringKeys.SqlDatabase} connection string not found.");
        }

        public async Task<IEnumerable<GenericSynonym>> GetLatestGenericSynonymsAsync(string category)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "category", category }
                };

                var result = await _dbContext.QueryAsync<GenericSynonym>(_connectionstring,
                                                                        StoredProcedures.GetLatestGenericSynonyms,
                                                                        parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetLatestGenericSynonymsAsync)} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
