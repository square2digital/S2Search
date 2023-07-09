using Domain.Customer.SearchResources.SearchInterfaces;
using Services.Customer.Interfaces.Managers;

namespace Services.Customer.Managers
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
