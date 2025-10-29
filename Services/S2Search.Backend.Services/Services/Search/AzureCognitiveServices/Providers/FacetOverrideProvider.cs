using S2Search.Backend.Domain.Interfaces.FacetOverrides;
using S2Search.Backend.Domain.Models.Facets;

namespace Services.Providers
{
    public class FacetOverrideProvider : IFacetOverrideProvider
    {
        private readonly IEnumerable<IFacetOverride> _facetOverrides;

        public FacetOverrideProvider(IEnumerable<IFacetOverride> facetOverrides)
        {
            _facetOverrides = facetOverrides ?? throw new ArgumentNullException(nameof(facetOverrides));
        }

        public FacetItem Override(string facetName, FacetItem item)
        {
            var facetOverride = _facetOverrides.FirstOrDefault(x => x.FacetName == facetName);

            if (facetOverride == null)
            {
                throw new NotImplementedException();
            }

            return facetOverride.Override(item);
        }
    }
}
