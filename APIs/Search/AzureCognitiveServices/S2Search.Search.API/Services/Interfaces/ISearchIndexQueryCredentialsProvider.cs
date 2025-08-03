using Domain.Configuration.SearchResources.Credentials;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISearchIndexQueryCredentialsProvider
    {
        Task<SearchIndexQueryCredentials> GetAsync(string callingHost);
    }
}