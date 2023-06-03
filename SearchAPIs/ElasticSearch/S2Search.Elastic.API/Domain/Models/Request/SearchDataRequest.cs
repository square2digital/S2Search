using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models.Request
{
    public class SearchDataRequest
    {
        [Required]
        [JsonPropertyName("searchTerm")]
        public string SearchTerm { get; set; }

        [Required]
        [JsonPropertyName("filters")]
        public string Filters { get; set; }

        [Required]
        [JsonPropertyName("orderBy")]
        public string OrderBy { get; set; }

        [Required]
        [JsonPropertyName("sortOrder")]
        public string SortOrder { get; set; }

        [Required]
        [JsonPropertyName("from")]
        public int From { get; set; }

        [Required]
        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("index")]
        public string Index { get; set; }
    }
}