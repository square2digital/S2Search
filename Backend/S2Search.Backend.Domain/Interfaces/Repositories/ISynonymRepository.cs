using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface ISynonymRepository
{
    Task<Synonym> CreateAsync(SynonymRequest synonymCreateRequest);
    Task<Synonym> UpdateAsync(SynonymRequest synonymUpdateRequest);
    Task DeleteAsync(Guid searchIndexId, Guid synonymId);
    Task<Synonym> GetByIdAsync(Guid searchIndexId, Guid synonymId);
    Task<Synonym> GetByKeyWordAsync(Guid searchIndexId, string keyWord);
    Task<IEnumerable<Synonym>> GetAsync(Guid searchIndexId);
}
