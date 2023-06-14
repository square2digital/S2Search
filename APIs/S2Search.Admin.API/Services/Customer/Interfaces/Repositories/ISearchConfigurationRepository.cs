using Domain.Customer.SearchResources.SearchConfiguration;

namespace Services.Customer.Interfaces.Repositories
{
    public interface ISearchConfigurationRepository
    {
        Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId);
        Task<int> UpdateConfigurationItem(SearchConfigurationUpdateMapping config);
    }
}
