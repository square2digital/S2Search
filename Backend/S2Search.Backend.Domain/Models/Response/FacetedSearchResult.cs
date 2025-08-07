using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Domain.Models.Response;

public class FacetedSearchResult
{
    public FacetedSearchResult() { }

    public FacetedSearchResult(IList<FacetGroup> facets)
    {
        Facets = facets;
    }

    public IList<FacetGroup> Facets { get; set; }
}
