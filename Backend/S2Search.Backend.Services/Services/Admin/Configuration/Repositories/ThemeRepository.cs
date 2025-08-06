using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Configuration.SearchResources;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.Themes;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Domain.Interfaces.Providers;

namespace S2Search.Backend.Services.Admin.Configuration.Repositories
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly IConfiguration _configuration;

        public ThemeRepository(IDbContextProvider dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Theme> GetThemeAsync(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            // Pass the key, not the value
            var connectionStringKey = "CustomerResourceStore";

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(
                connectionStringKey,
                StoredProcedures.GetTheme,
                parameters);

            return result;
        }

        public async Task<Theme> GetThemeById(Guid themeId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "ThemeId", themeId }
            };

            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(
                connectionString,
                StoredProcedures.GetThemeById,
                parameters);

            return result;
        }

        public async Task<ThemeCollection> GetThemesByCustomerId(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");

            var result = await _dbContext.QueryMultipleAsync<ThemeCollection>(
                connectionString,
                StoredProcedures.GetThemeByCustomerId,
                parameters);

            return result;
        }

        public async Task<Theme> GetThemeBySearchIndexId(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(
                connectionString,
                StoredProcedures.GetThemeBySearchIndexId,
                parameters);

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

            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");

            var result = await _dbContext.ExecuteAsync(
                connectionString,
                StoredProcedures.UpdateTheme,
                parameters);

            return result;
        }
    }
}
