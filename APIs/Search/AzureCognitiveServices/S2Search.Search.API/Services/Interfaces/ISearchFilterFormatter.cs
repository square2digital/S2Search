using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ISearchFilterFormatter
    {
        string Format(List<string> unformattedFilters);
    }
}