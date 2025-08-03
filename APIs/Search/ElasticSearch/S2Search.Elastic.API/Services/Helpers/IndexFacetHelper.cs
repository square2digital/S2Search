using Domain.Enums;
using Domain.Models.Facets;
using Newtonsoft.Json.Linq;

namespace Services.Helpers
{
    public static class IndexFacetHelper
    {
        public static List<ElasticFacet> BuildGenericFacet(string response, string index)
        {
            List<ElasticFacet> returnObj = new List<ElasticFacet>();

            JObject json = JObject.Parse(response);
            var schema = json[index];
            var mappings = schema["mappings"];

            if (mappings.HasValues)
            {
                var properties = mappings["properties"];

                foreach (var x in properties)
                {
                    var key = ((JProperty)x).Name;
                    var value = ((JProperty)x).Value;

                    ElasticFacet elasticFacet = new ElasticFacet();

                    var type = value["type"].ToString();

                    if (type == "integer")
                    {
                        // the interval will need to be passed as this can be very dependant on the field.
                        // also, not all integer types will be FacetType.range, many will be FacetType.terms

                        // if range
                        //elasticFacet = BuildElasticFacet(key, FacetType.range, key, 1000);
                        elasticFacet = BuildElasticFacet(key, FacetType.terms, key, null);
                    }

                    if (type == "double")
                    {
                        // like the comment above - params for the fact will need to come from a data source driven
                        // by the admin UI

                        // if range
                        elasticFacet = BuildElasticFacet(key, FacetType.range, key, 10000);
                    }

                    if (type == "text")
                    {
                        var fields = value["fields"];
                        if (fields == null) continue;

                        var raw = fields["raw"];

                        if (raw != null)
                        {
                            elasticFacet = BuildElasticFacet(key, FacetType.terms, $"{key}.raw", null);
                        }
                    }

                    if(!string.IsNullOrEmpty(elasticFacet.FacetName))
                    {
                        returnObj.Add(elasticFacet);
                    }
                }
            }

            return returnObj;
        }

        private static ElasticFacet BuildElasticFacet(string facetName, FacetType type, string field, int? interval)
        {
            ElasticFacet elasticFacet = new ElasticFacet();

            elasticFacet.FacetName = facetName;
            elasticFacet.Type = type.ToString();
            elasticFacet.FacetGroupName = $"group_by_{elasticFacet.FacetName}_{Enum.GetName(typeof(FacetType), type)}";
            elasticFacet.Field = field;
            elasticFacet.Interval = interval;

            return elasticFacet;
        }
    }
}
