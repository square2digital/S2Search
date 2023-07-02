using S2SearchAPI.Client;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISearchIndexQueryCredentialsProvider
    {
        Task<SearchIndexQueryCredentials> GetAsync(string callingHost);
    }
}