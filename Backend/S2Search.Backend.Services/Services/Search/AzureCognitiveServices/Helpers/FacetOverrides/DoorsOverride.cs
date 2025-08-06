using S2Search.Backend.Domain.Interfaces.FacetOverrides;
using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers.FacetOverrides
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
