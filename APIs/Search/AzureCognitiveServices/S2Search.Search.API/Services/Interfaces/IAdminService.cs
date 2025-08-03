using Domain.Configuration.SearchResources.Credentials;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<string>> GetGenericSynonyms(string category);
        Task<SearchIndexQueryCredentials> GetSearchIndexQueryCredentials(string customerEndpoint);
    }
}