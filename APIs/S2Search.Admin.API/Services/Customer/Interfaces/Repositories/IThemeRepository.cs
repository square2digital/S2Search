using Domain.Customer.SearchResources.Themes;

namespace Services.Customer.Interfaces.Repositories
{
    public interface IThemeRepository
    {
        Task<Theme> GetThemeById(Guid themeId);
        Task<Theme> GetThemeBySearchIndexId(Guid searchIndexId);
        Task<ThemeCollection> GetThemesByCustomerId(Guid customerId);
        Task<int> UpdateTheme(ThemeRequest theme);
    }
}