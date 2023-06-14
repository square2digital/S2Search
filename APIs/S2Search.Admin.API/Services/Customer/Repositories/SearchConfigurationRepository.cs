using Domain.Constants;
using Domain.Customer.Constants;
using Domain.Customer.SearchResources.SearchConfiguration;
using Services.Customer.Interfaces.Repositories;
using Services.Dapper.Interfaces.Providers;

namespace Services.Customer.Repositories
{
    public class SearchConfigurationRepository : ISearchConfigurationRepository
    {
        private readonly IDbContextProvider _dbContext;

        public SearchConfigurationRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var result = await _dbContext.QueryAsync<SearchConfigurationOption>(ConnectionStrings.CustomerResourceStore,
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

            var result = await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore,
                                                       StoredProcedures.InsertOrUpdateSearchConfigurationValueById,
                                                       parameters);

            return result;
        }
    }
}
