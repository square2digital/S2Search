using Domain.Models.Request;

namespace Domain.Interfaces
{
    public interface ISearchOptionsProvider
    {
        string CreateSearchOptions(SearchDataRequest request);
    }
}
