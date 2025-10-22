using Domain.Models;
using System.Collections.Generic;

namespace Tests.Builders
{
    internal class SearchFacetListBuilder
    {
        private List<SearchFacet> entity = new List<SearchFacet>();

        public SearchFacetListBuilder AddSearchFacets(params SearchFacet[] searchFacets)
        {
            entity.AddRange(searchFacets);
            return this;
        }

        public List<SearchFacet> Build()
        {
            return entity;
        }
    }
}
