using Domain.Customer.SearchResources.SearchInterfaces;

namespace Services.Customer.Interfaces.Managers
{
    public interface ISearchInterfaceValidationManager
    {
        bool IsValid(SearchInterfaceRequest searchInterfaceRequest, out string errorMessage);
    }
}
