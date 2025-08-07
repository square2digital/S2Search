using Azure.Search.Documents.Models;
using System.Text.Json.Serialization;

namespace S2Search.Backend.Domain.Models.Facets;

public class FacetItem
{
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("count")]
    public long? Count { get; set; }

    [JsonPropertyName("facetDisplayText")]
    public string FacetDisplayText { get; set; }

    [JsonPropertyName("selected")]
    public bool Selected { get; set; }

    public FacetItem() { }

    public FacetItem(FacetItem item) : this()
    {
        Value = item.Value;
        From = item.From;
        To = item.To;
        Count = item.Count;
        FacetDisplayText = item.FacetDisplayText;
        Selected = item.Selected;
    }

    public FacetItem(FacetResult result) : this()
    {
        if (result.FacetType == FacetType.Range)
        {
            Value = result.Value?.ToString();
            From = result.From?.ToString();
            To = result.To?.ToString();
            Count = result.Count;
        }
        else
        {
            Value = result.Value?.ToString();
            From = null;
            To = null;
            Count = result.Count;
        }

        Type = result.FacetType.ToString();
    }
}