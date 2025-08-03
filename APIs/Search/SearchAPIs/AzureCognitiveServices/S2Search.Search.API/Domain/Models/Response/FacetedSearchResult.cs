using Domain.Models.Facets;
using System.Collections.Generic;

namespace Domain.Models.Response
{
    public class FacetedSearchResult
    {
        public FacetedSearchResult() { }

        public FacetedSearchResult(IList<FacetGroup> facets)
        {
            this.Facets = facets;
        }

        public IList<FacetGroup> Facets { get; set; }
    }
}
