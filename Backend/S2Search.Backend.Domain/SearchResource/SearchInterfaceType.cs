using System.ComponentModel;

namespace S2Search.Backend.Domain.SearchResource
{
    public enum SearchInterfaceType
    {
        [Description("Custom UI")]
        Custom_UI = 1,

        [Description("API Consumption")]
        API_Consumption = 2
    }
}
