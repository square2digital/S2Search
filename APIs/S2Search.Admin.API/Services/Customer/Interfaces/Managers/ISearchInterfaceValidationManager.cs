using Domain.SearchResources.SearchInterfaces;

namespace Services.Interfaces.Managers
{
    public interface ISearchInterfaceValidationManager
    {
        bool IsValid(SearchInterfaceRequest searchInterfaceRequest, out string errorMessage);
    }
}
