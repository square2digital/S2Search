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
        private readonly string _connectionString;

        public ThemeRepository(IDbContextProvider dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = configuration.GetConnectionString("S2_Search");
        }

        public async Task<Theme> GetThemeAsync(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerEndpoint", customerEndpoint }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(
                _connectionString,
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

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(
                _connectionString,
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

            var result = await _dbContext.QueryMultipleAsync<ThemeCollection>(
                _connectionString,
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

            var result = await _dbContext.QuerySingleOrDefaultAsync<Theme>(
                _connectionString,
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

            var result = await _dbContext.ExecuteAsync(
                _connectionString,
                StoredProcedures.UpdateTheme,
                parameters);

            return result;
        }
    }
}
