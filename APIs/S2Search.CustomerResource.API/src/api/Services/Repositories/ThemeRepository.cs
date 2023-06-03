using Domain.Constants;
using Domain.SearchResources.Themes;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<Theme> GetThemeById(Guid themeId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "ThemeId", themeId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetThemeById, parameters);

            return result;
        }

        public async Task<ThemeCollection> GetThemesByCustomerId(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QueryMultipleAsync<ThemeCollection>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetThemeByCustomerId, parameters);

            return result;
        }

        public async Task<Theme> GetThemeBySearchIndexId(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetThemeBySearchIndexId, parameters);

            return result;
        }

        public async Task<int> UpdateTheme(ThemeRequest theme)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "ThemeId", theme.themeId },
                { "PrimaryHexColour", theme.primaryHexColour },
                { "SecondaryHexColour", theme.secondaryHexColour },
                { "NavBarHexColour", theme.navBarHexColour },
                { "LogoURL", theme.logoURL },
                { "MissingImageURL", theme.MissingImageURL }
            };

            var result = await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.UpdateTheme, parameters);

            return result;
        }
    }
}
