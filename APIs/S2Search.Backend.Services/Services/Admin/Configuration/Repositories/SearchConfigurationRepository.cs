using S2Search.Backend.Domain.Configuration.SearchResources.Configuration;
using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.SearchConfiguration;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Domain.Interfaces.Providers;

namespace S2Search.Backend.Services.Admin.Configuration.Repositories
{
    public class SearchConfigurationRepository : ISearchConfigurationRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SearchConfigurationRepository(IDbContextProvider dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = configuration.GetConnectionString("S2_Search");
        }

        public async Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var result = await _dbContext.QueryAsync<SearchConfigurationOption>(
                _connectionString,
                StoredProcedures.GetConfigurationForSearchIndex,
                parameters);

            return result;
        }

        public async Task<int> UpdateConfigurationItem(SearchConfigurationUpdateMapping config)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchConfigurationMappingId", config.SearchConfigurationMappingId },
                { "SeachConfigurationOptionId", config.SeachConfigurationOptionId },
                { "SearchIndexId", config.SearchIndexId },
                { "Value", config.Value }
            };

            var result = await _dbContext.ExecuteAsync(
                _connectionString,
                StoredProcedures.InsertOrUpdateSearchConfigurationValueById,
                parameters);

            return result;
        }
    }
}