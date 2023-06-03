using Azure.Search.Documents.Models;
using System.Text.Json.Serialization;

namespace Domain.Models.Facets
{
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
            this.Value = item.Value;
            this.From = item.From;
            this.To = item.To;
            this.Count = item.Count;
            this.FacetDisplayText = item.FacetDisplayText;
            this.Selected = item.Selected;
        }

        public FacetItem(FacetResult result) : this()
        {
            if (result.FacetType == FacetType.Range)
            {
                this.Value = result.Value?.ToString();
                this.From = result.From?.ToString();
                this.To = result.To?.ToString();
                this.Count = result.Count;
            }
            else
            {
                this.Value = result.Value?.ToString();
                this.From = null;
                this.To = null;
                this.Count = result.Count;
            }

            this.Type = result.FacetType.ToString();
        }
    }
}