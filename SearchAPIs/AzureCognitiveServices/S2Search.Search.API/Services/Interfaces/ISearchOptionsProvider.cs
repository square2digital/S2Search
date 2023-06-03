using Azure.Search.Documents;
using Domain.Models.Request;

namespace Services.Interfaces
{
    public interface ISearchOptionsProvider
    {
        SearchOptions CreateSearchOptions(SearchRequest request);
    }
}