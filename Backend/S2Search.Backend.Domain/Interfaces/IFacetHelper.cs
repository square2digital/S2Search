using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Domain.Interfaces;

public interface IFacetHelper
{
    IList<FacetGroup> SetFacetOrder(IList<FacetGroup> Facets);
    IList<FacetGroup> GetDefaultFacetsFromLocal();
    IList<FacetGroup> GetDefaultFacetsFromOneDrive();
}