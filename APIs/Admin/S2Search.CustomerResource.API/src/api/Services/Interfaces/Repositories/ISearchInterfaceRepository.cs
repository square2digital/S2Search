using Domain.SearchResources.SearchInterfaces;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchInterfaceRepository
    {
        Task<SearchInterface> CreateAsync(SearchInterfaceRequest searchInterfaceRequest);
        Task<SearchInterface> GetLatestAsync(Guid searchIndexId);
    }
}
