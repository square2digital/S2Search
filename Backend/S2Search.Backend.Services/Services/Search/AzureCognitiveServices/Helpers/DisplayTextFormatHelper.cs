using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers
{
    public class DisplayTextFormatHelper : IDisplayTextFormatHelper
    {
        private readonly IAppSettings _appSettings;

        public DisplayTextFormatHelper(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public string FormatNonCurrencyRange(string from, string to)
        {
            string displayText = string.Empty;

            if (string.IsNullOrEmpty(from) || from == "0")
            {
                displayText = string.Format("{0} and below", Convert.ToDouble(to)).Trim();
            }

            else if (string.IsNullOrEmpty(to) || to == _appSettings.SearchSettings.FacetMaxRangeToValue.ToString())
            {
                displayText = string.Format("{0} and above", Convert.ToDouble(from)).Trim();
            }

            else
            {
                displayText = string.Format("{0} to {1}", Convert.ToDouble(from), Convert.ToDouble(to)).Trim();
            }

            return displayText;
        }

        public string FormatCurrencyRange(string from, string to, string prefix, string unit)
        {
            string displayText = string.Empty;

            if (unit != string.Empty)
            {
                unit = string.Format(" {0}", unit);
            }

            if (string.IsNullOrEmpty(from) || from == "0")
            {
                displayText = string.Format("{0}{1:N0}{2} and below", prefix, Convert.ToDouble(to), unit).Trim();
            }

            else if (string.IsNullOrEmpty(to) || to == _appSettings.SearchSettings.FacetMaxRangeToValue.ToString())
            {
                displayText = string.Format("{0}{1:N0}{2} and above", prefix, Convert.ToDouble(from), unit).Trim();
            }

            else
            {
                displayText = string.Format("{0}{1:N0} to {0}{2:N0}{3}", prefix, Convert.ToDouble(from), Convert.ToDouble(to), unit).Trim();
            }

            return displayText;
        }

        public void SetFacetGroupDisplayNames(FacetGroup group, FacetGroup NewGroup)
        {
            if (group.FacetName.Equals("monthlyPrice", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Monthly Price";
                return;
            }
            if (group.FacetName.Equals("bodyStyle", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Body Style";
                return;
            }
            if (group.FacetName.Equals("doors", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Doors";
                return;
            }
            if (group.FacetName.Equals("model", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Model";
                return;
            }
            if (group.FacetName.Equals("variant", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Variant";
            }
            if (group.FacetName.Equals("make", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Make";
                return;
            }
            if (group.FacetName.Equals("colour", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Colour";
                return;
            }
            if (group.FacetName.Equals("price", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Price";
                return;
            }
            if (group.FacetName.Equals("fuelType", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Fuel Type";
                return;
            }
            if (group.FacetName.Equals("location", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Location";
                return;
            }
            if (group.FacetName.Equals("engineSize", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Engine Size";
                return;
            }
            if (group.FacetName.Equals("transmission", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Transmission";
                return;
            }
            if (group.FacetName.Equals("mileage", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Mileage";
                return;
            }
            if (group.FacetName.Equals("year", StringComparison.OrdinalIgnoreCase))
            {
                NewGroup.FacetKeyDisplayName = "Year";
                return;
            }
        }
    }
}
