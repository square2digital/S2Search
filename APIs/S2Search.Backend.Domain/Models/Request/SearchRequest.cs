using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace S2Search.Backend.Domain.Models.Request;

public class SearchRequest
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
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [Required]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("numberOfExistingResults")]
    public int NumberOfExistingResults { get; set; }

    [JsonPropertyName("customerEndpoint")]
    public string CustomerEndpoint { get; set; }
}