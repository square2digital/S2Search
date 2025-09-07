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

        public SearchConfigurationRepository(IDbContextProvider dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var connectionString = _configuration.GetConnectionString("S2_Search");

            var result = await _dbContext.QueryAsync<SearchConfigurationOption>(
                connectionString,
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

            var connectionString = _configuration.GetConnectionString("S2_Search");

            var result = await _dbContext.ExecuteAsync(
                connectionString,
                StoredProcedures.InsertOrUpdateSearchConfigurationValueById,
                parameters);

            return result;
        }
    }
}