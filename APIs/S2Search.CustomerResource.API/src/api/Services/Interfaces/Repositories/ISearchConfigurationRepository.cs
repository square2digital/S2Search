using Domain.SearchResources.SearchConfiguration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchConfigurationRepository
    {
        Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId);
        Task<int> UpdateConfigurationItem(SearchConfigurationUpdateMapping config);
    }
}
