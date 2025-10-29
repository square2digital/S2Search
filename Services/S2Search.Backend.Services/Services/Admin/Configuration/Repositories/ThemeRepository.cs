using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Configuration.SearchResources;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.Themes;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Domain.Interfaces.Repositories;

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
            _connectionString = configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase);
        }

        public async Task<Theme> GetThemeAsync(string customerEndpoint)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "customer_endpoint", customerEndpoint }
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
                { "theme_id", themeId }
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
                { "customer_id", customerId }
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
                { "search_index_id", searchIndexId }
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
                { "theme_id", theme.themeId },
                { "primary_hex_colour", theme.primaryHexColour },
                { "secondary_hex_colour", theme.secondaryHexColour },
                { "nav_bar_hex_colour", theme.navBarHexColour },
                { "logo_url", theme.logoURL },
                { "missing_image_url", theme.MissingImageURL }
            };

            var result = await _dbContext.ExecuteAsync(
                _connectionString,
                StoredProcedures.UpdateTheme,
                parameters);

            return result;
        }
    }
}
