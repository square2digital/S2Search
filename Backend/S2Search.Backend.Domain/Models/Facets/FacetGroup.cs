using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace S2Search.Backend.Domain.Models.Facets;

public class FacetGroup
{
    public FacetGroup()
    {
        FacetItems = new List<FacetItem>();
    }

    public FacetGroup(FacetGroup group) : this()
    {
        FacetName = group.FacetName;
        FacetKeyDisplayName = group.FacetKeyDisplayName;
        FacetKey = group.FacetKey;
        FacetItems = group.FacetItems;
        Enabled = group.Enabled;
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
}