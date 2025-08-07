using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface ISearchInterfaceValidationManager
{
    bool IsValid(SearchInterfaceRequest searchInterfaceRequest, out string errorMessage);
}
