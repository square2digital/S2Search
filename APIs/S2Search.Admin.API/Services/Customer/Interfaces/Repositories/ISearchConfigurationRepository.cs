using Domain.Customer.SearchResources.SearchConfiguration;
using Domain.SearchResources.Configuration;

namespace Services.Customer.Interfaces.Repositories
{
    public interface ISearchConfigurationRepository
    {
        Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId);
        Task<int> UpdateConfigurationItem(SearchConfigurationUpdateMapping config);
    }
}
