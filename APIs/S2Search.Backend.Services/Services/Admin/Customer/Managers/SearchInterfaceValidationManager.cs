using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class SearchInterfaceValidationManager : ISearchInterfaceValidationManager
    {
        public bool IsValid(SearchInterfaceRequest searchInterfaceRequest, out string errorMessage)
        {
            errorMessage = "";
            if (!Enum.TryParse(searchInterfaceRequest.SearchInterfaceType, out SearchInterfaceType _))
            {
                errorMessage = $"Invalid {nameof(searchInterfaceRequest.SearchInterfaceType)}";
                return false;
            }

            return true;
        }
    }
}
