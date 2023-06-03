using Domain.Models.Facets;
using Domain.Models.Request;
using Domain.Models.Response;
using S2Search.ClientConfigurationApi.Client.AutoRest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAzureSearchService
    {
        Task<SearchResultRoot> InvokeSearchRequest(SearchRequest request, SearchIndexQueryCredentials targetSearchResource);
        Task<int> TotalDocumentCount(SearchIndexQueryCredentials targetSearchResource);
        Task<IList<FacetGroup>> GetDefaultFacets(string callingHost, SearchIndexQueryCredentials queryCredentials);

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
        /// <exception cref="System.ArgumentNullException">Thrown when the searchTerm is null or empty</exception>
        Task<IEnumerable<string>> AutocompleteWithSuggestions(string searchTerm, SearchIndexQueryCredentials queryCredentials);
    }
}