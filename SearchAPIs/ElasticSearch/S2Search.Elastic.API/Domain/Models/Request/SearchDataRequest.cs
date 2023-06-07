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
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("index")]
        public string Index { get; set; }
    }
}