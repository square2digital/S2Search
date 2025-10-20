using Domain.Models;
using System.Collections.Generic;

namespace Services.Interfaces.Managers
{
    public interface ISearchFacetsFormatManager
    {
        public IEnumerable<SearchFacet> GetSearchFacets(string unformattedFacets);
    }
}
