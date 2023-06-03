using Domain.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Models.Facets
{
    public class ElasticFacet
    {
        public ElasticFacet() { }

        public ElasticFacet(string facetName,
           FacetType type,
           string facetGroupName,
           string field,
           int? interval
           ) : this()
        {
            FacetName = facetName;
            Type = type;
            FacetGroupName = facetGroupName;
            Field = field;
            Interval = interval;
        }

        public string FacetName { get; set; }        
        public FacetType Type { get; set; }
        public string FacetGroupName { get; set; }
        public string Field { get; set; }
        public int? Interval { get; set; }
    }
}