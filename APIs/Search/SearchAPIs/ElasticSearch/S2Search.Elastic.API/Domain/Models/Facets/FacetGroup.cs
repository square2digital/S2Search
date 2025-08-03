using System.Text.Json.Serialization;

namespace Domain.Models.Facets
{
    public class FacetGroup
    {
        public FacetGroup()
        {
            FacetItems = new List<FacetItem>();
        }

        public FacetGroup(FacetGroup group) : this()
        {
            this.FacetName = group.FacetName;
            this.FacetKeyDisplayName = group.FacetKeyDisplayName;
            this.FacetKey = group.FacetKey;
            this.FacetItems = group.FacetItems;
            this.Enabled = group.Enabled;
            this.Type = group.Type;
        }

        [JsonPropertyName("facetName")]
        public string FacetName { get; set; }

        [JsonPropertyName("facetKeyDisplayName")]
        public string FacetKeyDisplayName { get; set; }

        [JsonPropertyName("facetKey")]
        public string FacetKey { get; set; }

        [JsonPropertyName("facetItems")]
        public List<FacetItem> FacetItems { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}