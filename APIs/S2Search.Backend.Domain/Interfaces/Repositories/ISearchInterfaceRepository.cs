using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface ISearchInterfaceRepository
{
    Task<SearchInterface> CreateAsync(SearchInterfaceRequest searchInterfaceRequest);
    Task<SearchInterface> GetLatestAsync(Guid searchIndexId);
}
