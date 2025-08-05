using Domain.Constants;
using Domain.Customer.Constants;
using Domain.Customer.SearchResources.SearchConfiguration;
using Domain.SearchResources.Configuration;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Configuration.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Services.Configuration.Repositories
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

            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");

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

            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");

            var result = await _dbContext.ExecuteAsync(
                connectionString,
                StoredProcedures.InsertOrUpdateSearchConfigurationValueById,
                parameters);

            return result;
        }
    }
}