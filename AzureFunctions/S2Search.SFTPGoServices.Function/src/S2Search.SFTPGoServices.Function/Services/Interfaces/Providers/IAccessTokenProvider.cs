using System.Threading.Tasks;

namespace Services.Interfaces.Providers
{
    public interface IAccessTokenProvider
    {
        Task<string> GetOrRefreshAccessTokenAsync();
    }
}
