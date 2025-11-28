using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Request;
using S2Search.Backend.Domain.Models.Response;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

public interface IAzureSearchService
{
    Task<SearchResultRoot> InvokeSearchRequest(SearchRequest request, SearchIndexQueryCredentials queryCredentials);
    Task<int> TotalDocumentCount(SearchIndexQueryCredentials queryCredentials);

    /// <summary>
    /// Returns a list containing an autocomplete result first, followed by suggestions that match the <paramref name="searchTerm"/> provided.
    /// <para>
    /// For example, the <paramref name="searchTerm"/> "fo" could result in the following:
    /// </para>
    /// <list type="number">
    /// <item><description>ford (The autocomplete result. This will always be the first item in the list)</description></item>
    /// <item><description>Ford Fiesta</description></item>
    /// <item><description>Ford Focus</description></item>
    /// </list>
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the searchTerm is null or empty</exception>
    Task<IEnumerable<string>> AutocompleteWithSuggestions(string searchTerm, SearchIndexQueryCredentials queryCredentials);
}