using Domain.Models.Facets;
using Services.Interfaces.FacetOverrides;
using System;
using System.Collections.Generic;
using System.Linq;

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

            if(facetOverride == null)
            {
                throw new NotImplementedException();
            }

            return facetOverride.Override(item);
        }
    }
}
