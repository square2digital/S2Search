using S2Search.Backend.Domain.AzureSearch;
using System.Collections.Generic;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

public interface IAzureAutoSuggestOptionsProvider
{
    /// <summary>
    /// Gets the options to be applied to an Azure Search Autocomplete with Suggestions request.
    /// </summary>
    /// <param name="suggesterName">The name of the suggester applied to the Index, this must be provided.</param>
    /// <param name="suggestSelectFields">A list of field names to be returned from an index document. Passing <c>null</c> will just return the Key for the index e.g. vehicleId</param>
    /// <param name="suggestSearchFields">A list of field names to be searched. Passing <c>null</c> will result in all suggester-aware fields being searched.</param>
    /// <param name="autocompleteSearchFields">A list of field names to be searched. Passing <c>null</c> will result in all suggester-aware fields being searched.</param>
    /// <returns></returns>
    AzureAutoSuggestOptions Get(string suggesterName,
                                IEnumerable<string> suggestSelectFields,
                                IEnumerable<string> suggestSearchFields,
                                IEnumerable<string> autocompleteSearchFields);
}