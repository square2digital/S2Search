using Domain.SearchResources;
using System.Threading.Tasks;

namespace Services.Configuration.Interfaces.Repositories
{
    public interface IThemeRepository
    {
        Task<Theme> GetThemeAsync(string customerEndpoint);
    }
}
