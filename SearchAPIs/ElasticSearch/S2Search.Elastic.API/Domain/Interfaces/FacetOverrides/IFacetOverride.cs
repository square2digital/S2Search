using Domain.Models.Facets;

namespace Services.Interfaces.FacetOverrides
{
    public interface IFacetOverride
    {
        string FacetName { get; }

        FacetItem Override(FacetItem item);
    }
}