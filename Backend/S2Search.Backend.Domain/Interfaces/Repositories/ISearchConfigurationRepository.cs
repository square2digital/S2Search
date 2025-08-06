using S2Search.Backend.Domain.Configuration.SearchResources.Configuration;
using S2Search.Backend.Domain.Customer.SearchResources.SearchConfiguration;

namespace S2Search.Backend.Domain.Interfaces.Repositories
{
    public interface ISearchConfigurationRepository
    {
        Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId);
        Task<int> UpdateConfigurationItem(SearchConfigurationUpdateMapping config);
    }
}
