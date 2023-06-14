using Domain.Constants;
using Domain.Customer.Constants;
using Domain.SearchResources;
using Services.Configuration.Interfaces.Repositories;
using Services.Dapper.Interfaces.Providers;

namespace Services.Configuration.Repositories
{
    public class SearchIndexRepository : ISearchIndexRepository
    {
        private readonly IDbContextProvider _dbContext;

        public SearchIndexRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<SearchIndexQueryCredentials> GetQueryCredentialsAsync(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexQueryCredentials>(ConnectionStrings.CustomerResourceStore,
                                                                                 StoredProcedures.GetSearchIndexQueryCredentials,
                                                                                 parameters);

            return result;
        }

        public async Task<IEnumerable<GenericSynonyms>> GetGenericSynonymsByCategoryAsync(string category)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "Category", category }
            };

            var result = await _dbContext.QueryAsync<GenericSynonyms>(ConnectionStrings.CustomerResourceStore,
                                                                                 Domain.Configuration.Constants.StoredProcedures.GetGenericSynonymsByCategory,
                                                                                 parameters);

            return result;
        }
    }
}
