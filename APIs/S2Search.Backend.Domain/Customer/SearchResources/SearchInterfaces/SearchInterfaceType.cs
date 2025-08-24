using System.ComponentModel;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;

public enum SearchInterfaceType
{
    [Description("Custom UI")]
    Custom_UI = 1,

    [Description("API Consumption")]
    API_Consumption = 2
}
