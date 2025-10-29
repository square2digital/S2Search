using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Configuration.SearchResources.Synonyms;
using S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

namespace S2Search.Backend.Domain.Interfaces.Repositories;

public interface ISearchIndexRepository
{
    //Task<IEnumerable<GenericSynonyms>> GetGenericSynonymsByCategoryAsync(string category);
    void Create(SearchIndexRequest indexRequest);
    Task CreateAsync(SearchIndexRequest indexRequest);
    Task<SearchIndex> GetAsync(Guid customerId, Guid searchIndexId);
    Task<SearchIndex> GetByFriendlyNameAsync(Guid customerId, string friendlyName);
    Task<SearchIndexFull> GetFullAsync(Guid customerId, Guid searchIndexId);
    void Delete(Guid searchIndexId);
    Task DeleteAsync(Guid searchIndexId);
    //Task<IEnumerable<SearchIndexKeys>> GetKeysAsync(Guid customerId, Guid searchIndexId);
    Task CreateKeysAsync(SearchIndexKeyGenerationRequest keyGenerationRequest);
    Task DeleteKeysAsync(SearchIndexKeyDeletionRequest keyDeletionRequest);
    Task<SearchIndexQueryCredentials> GetQueryCredentials(string customerEndpoint);
}
