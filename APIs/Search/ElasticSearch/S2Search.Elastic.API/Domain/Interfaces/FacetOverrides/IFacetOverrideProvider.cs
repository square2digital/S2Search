using Domain.Models.Facets;

namespace Services.Interfaces.FacetOverrides
{
    public interface IFacetOverrideProvider
    {
        FacetItem Override(string facetName, FacetItem item);
    }
}
