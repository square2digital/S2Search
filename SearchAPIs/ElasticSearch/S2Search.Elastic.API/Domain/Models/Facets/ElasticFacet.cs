using Domain.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Models.Facets
{
    public class ElasticFacet
    {
        public ElasticFacet() { }

        public ElasticFacet(string facetKey,
           string facetName,
           string type,
           string facetGroupName,
           string field,
           int? interval,
           int? maxValue
           ) : this()
        {
            FacetKey = facetKey;
            FacetName = facetName;            
            Type = type;
            FacetGroupName = facetGroupName;
            Field = field;
            Interval = interval;
            MaxValue = maxValue;
        }

        public string FacetKey { get; set; }
        public string FacetName { get; set; }        
        public string Type { get; set; }
        public string FacetGroupName { get; set; }
        public string Field { get; set; }
        public int? Interval { get; set; }
        public int? MaxValue { get; set; }
    }
}