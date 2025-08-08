using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Domain.Interfaces;

public interface IDisplayTextFormatHelper
{
    string FormatCurrencyRange(string from, string to, string prefix, string unit);
    string FormatNonCurrencyRange(string from, string to);
    void SetFacetGroupDisplayNames(FacetGroup group, FacetGroup NewGroup);
}