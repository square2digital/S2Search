using Domain.Customer.SearchResources.SearchInterfaces;

namespace Services.Customer.Interfaces.Repositories
{
    public interface ISearchInterfaceRepository
    {
        Task<SearchInterface> CreateAsync(SearchInterfaceRequest searchInterfaceRequest);
        Task<SearchInterface> GetLatestAsync(Guid searchIndexId);
    }
}
