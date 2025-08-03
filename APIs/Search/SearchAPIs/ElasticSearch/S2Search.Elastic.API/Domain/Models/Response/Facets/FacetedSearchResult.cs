using Domain.Models.Facets;
using System.Collections.Generic;

namespace Domain.Models.Response.Facets
{
    public class FacetedSearchResult
    {
        public FacetedSearchResult() { }

        public FacetedSearchResult(IList<FacetGroup> facets)
        {
            Facets = facets;
        }

        public IList<FacetGroup> Facets { get; set; }
    }
}
