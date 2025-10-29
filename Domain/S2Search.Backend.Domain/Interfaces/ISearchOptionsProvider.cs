using Azure.Search.Documents;
using S2Search.Backend.Domain.Models.Request;

namespace S2Search.Backend.Domain.Interfaces;

public interface ISearchOptionsProvider
{
    SearchOptions CreateSearchOptions(SearchRequest request);
}