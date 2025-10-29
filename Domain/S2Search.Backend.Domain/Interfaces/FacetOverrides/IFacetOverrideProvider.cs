using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Domain.Interfaces.FacetOverrides;

public interface IFacetOverrideProvider
{
    FacetItem Override(string facetName, FacetItem item);
}
