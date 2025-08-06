using S2Search.Backend.Domain.Configuration.SearchResources;
using S2Search.Backend.Domain.Customer.SearchResources.Themes;

namespace S2Search.Backend.Domain.Interfaces.Repositories
{
    public interface IThemeRepository
    {
        Task<Theme> GetThemeAsync(string customerEndpoint);
        Task<Theme> GetThemeById(Guid themeId);
        Task<Theme> GetThemeBySearchIndexId(Guid searchIndexId);
        Task<ThemeCollection> GetThemesByCustomerId(Guid customerId);
        Task<int> UpdateTheme(ThemeRequest theme);
    }
}
