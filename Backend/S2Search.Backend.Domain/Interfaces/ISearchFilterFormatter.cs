using System.Collections.Generic;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

public interface ISearchFilterFormatter
{
    string Format(List<string> unformattedFilters);
}