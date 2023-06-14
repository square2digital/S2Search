using Domain.SearchResources.Themes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IThemeRepository
    {
        Task<Theme> GetThemeById(Guid themeId);
        Task<Theme> GetThemeBySearchIndexId(Guid searchIndexId);
        Task<ThemeCollection> GetThemesByCustomerId(Guid customerId);
        Task<int> UpdateTheme(ThemeRequest theme);
    }
}