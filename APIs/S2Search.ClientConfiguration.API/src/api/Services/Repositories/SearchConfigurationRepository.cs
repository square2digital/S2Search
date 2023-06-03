using Domain.Constants;
using Domain.SearchResources.Configuration;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
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
    }
}