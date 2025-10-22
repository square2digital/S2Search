using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Logging;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
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

                var result = await _dbContext.QuerySingleOrDefaultAsync<SearchIndexCredentials>(ConnectionStrings.CustomerResourceStore,
                                                                                StoredProcedures.GetSearchIndexCredentials,
                                                                                parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCredentials)} | Message: {ex.Message}");
                throw ex;
            }
        }
    }
}
