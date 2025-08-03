using Domain.Customer.SearchResources.SearchConfiguration;
using Domain.SearchResources.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Configuration.Interfaces.Repositories
{
    public interface ISearchConfigurationRepository
    {
        Task<IEnumerable<SearchConfigurationOption>> GetConfigurationForSearchIndexAsync(Guid searchIndexId);
        Task<int> UpdateConfigurationItem(SearchConfigurationUpdateMapping config);
    }
}