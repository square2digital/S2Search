using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Domain.Interfaces.FacetOverrides
{
    public interface IFacetOverride
    {
        string FacetName { get; }

        FacetItem Override(FacetItem item);
    }
}