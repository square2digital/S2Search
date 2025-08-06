using S2Search.Backend.Domain.Interfaces.FacetOverrides;
using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers.FacetOverrides
{
    public class MileageOverride : IFacetOverride
    {
        private string milesStr = "miles";

        public string FacetName => "mileage";

        public FacetItem Override(FacetItem item)
        {
            if (string.IsNullOrEmpty(item.From) && !string.IsNullOrEmpty(item.To))
            {
                item.FacetDisplayText = $"Below {ConvertMileageToText(item.To)} {milesStr}";
                return item;
            }

            if (!string.IsNullOrEmpty(item.From) || !string.IsNullOrEmpty(item.To))
            {
                item.FacetDisplayText = $"{ConvertMileageToText(item.From)} - {ConvertMileageToText(item.To)} {milesStr}";
                return item;
            }

            if (!string.IsNullOrEmpty(item.From) && string.IsNullOrEmpty(item.To))
            {
                item.FacetDisplayText = $"Above {ConvertMileageToText(item.To)} {milesStr}";
                return item;
            }

            return item;
        }

        private string ConvertMileageToText(string value)
        {
            return $"{Convert.ToInt32(value) / 1000}k";
        }
    }
}
