using Nest;
using System.Linq;
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

        public FacetItem()
        {
            this.From = null;
            this.To = null;
        }

        public FacetItem(FacetItem item) : this()
        {
            this.Value = item.Value;
            this.From = item.From;
            this.To = item.To;
            this.Count = item.Count;
            this.FacetDisplayText = item.FacetDisplayText;
            this.Selected = item.Selected;
        }

        public FacetItem(KeyedBucket<string> item) : this()
        {
            this.Value = item.Key.ToString();
            //this.From = null;
            //this.To = null;
            this.Count = item.DocCount;
            this.FacetDisplayText = item.Key.ToString();
            //this.Selected = item.Selected;
        }

        public FacetItem(RangeBucket item) : this()
        {
            this.Value = item.Key.ToString();

            if (item.From != null)
            {
                this.From = item.From.Value.ToString();
            }

            if (item.To != null)
            {
                this.To = item.To.Value.ToString();
            }

            this.Count = item.DocCount;
            this.FacetDisplayText = item.Key.ToString();
            //this.Selected = item.Selected;
        }
    }
}