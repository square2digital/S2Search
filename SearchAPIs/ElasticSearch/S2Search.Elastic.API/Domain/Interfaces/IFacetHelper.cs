using Domain.Models.Facets;

namespace Domain.Interfaces
{
    public interface IFacetHelper
    {
        IList<FacetGroup> SetFacetOrder(IList<FacetGroup> Facets);
        IList<FacetGroup> GetDefaultFacetsFromLocal();        
    }
}