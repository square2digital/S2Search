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
    public class GenericSynonymRepository : IGenericSynonymRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly ILogger _logger;

        public GenericSynonymRepository(IDbContextProvider dbContext,
                                        ILogger<GenericSynonymRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<GenericSynonym>> GetLatestGenericSynonymsAsync(string category)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "Category", category }
                };

                var result = await _dbContext.QueryAsync<GenericSynonym>(ConnectionStrings.CustomerResourceStore,
                                                                                StoredProcedures.GetLatestGenericSynonyms,
                                                                                parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetLatestGenericSynonymsAsync)} | Message: {ex.Message}");
                throw ex;
            }
        }
    }
}
