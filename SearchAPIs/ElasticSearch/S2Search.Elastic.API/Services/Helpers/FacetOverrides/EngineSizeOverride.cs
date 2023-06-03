using Domain.Models.Facets;
using Services.Interfaces.FacetOverrides;
using System;

namespace Services.Helpers.FacetOverrides
{
    public class EngineSizeOverride : IFacetOverride
    {
        public string FacetName => "engineSize";

        public FacetItem Override(FacetItem item)
        {
            if (!string.IsNullOrEmpty(item.Value))
            {
                decimal rounded = RoundUpToNearestHundred(item.Value);
                decimal decimalRepresentation = (rounded / 1000.0M);
                string displayText = $"{decimalRepresentation.ToString("0.##")} L"; 

                item.Value = rounded.ToString();
                item.FacetDisplayText = displayText;
            }

            return item;
        }

        private decimal RoundUpToNearestHundred(string value)
        {
            return Math.Round(Convert.ToDecimal(value) / 100M, 0) * 100;
        }
    }
}
