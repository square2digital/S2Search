using Domain.Models.Facets;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IFacetHelper
    {
        IList<FacetGroup> GetDefaultFacetsFromLocal();
        IList<FacetGroup> GetDefaultFacetsFromOneDrive();
        IList<FacetGroup> SetFacetOrder(IList<FacetGroup> Facets);
    }
}