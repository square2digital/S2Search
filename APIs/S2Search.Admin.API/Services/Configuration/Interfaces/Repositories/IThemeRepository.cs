using Domain.Customer.SearchResources.Themes;
using Domain.SearchResources;
using System.Threading.Tasks;

namespace Services.Configuration.Interfaces.Repositories
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
