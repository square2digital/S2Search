using Domain.SearchResources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchIndexRepository
    {
        Task<SearchIndexQueryCredentials> GetQueryCredentialsAsync(string customerEndpoint);
        Task<IEnumerable<GenericSynonyms>> GetGenericSynonymsByCategoryAsync(string category);
    }
}
