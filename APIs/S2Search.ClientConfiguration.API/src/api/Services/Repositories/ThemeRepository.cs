using Domain.Constants;
using Domain.SearchResources;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly IDbContextProvider _dbContext;

        public ThemeRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Theme> GetThemeAsync(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(ConnectionStrings.CustomerResourceStore,
                                                                           StoredProcedures.GetTheme,
                                                                           parameters);

            return result;
        }
    }
}
