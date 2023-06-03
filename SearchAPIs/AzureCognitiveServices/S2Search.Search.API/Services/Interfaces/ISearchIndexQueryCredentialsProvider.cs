using S2Search.ClientConfigurationApi.Client.AutoRest.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISearchIndexQueryCredentialsProvider
    {
        Task<SearchIndexQueryCredentials> GetAsync(string callingHost);
    }
}