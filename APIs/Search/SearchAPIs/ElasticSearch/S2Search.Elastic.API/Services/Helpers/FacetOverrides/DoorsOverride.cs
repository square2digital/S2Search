using Domain.Models.Facets;
using Services.Interfaces.FacetOverrides;

namespace Services.Helpers.FacetOverrides
{
    public class DoorsOverride : IFacetOverride
    {
        public string FacetName => "doors";

        public FacetItem Override(FacetItem item)
        {
            if (!string.IsNullOrEmpty(item.Value))
            {
                item.FacetDisplayText = $"{item.Value} Doors";
            }

            return item;
        }
    }
}
