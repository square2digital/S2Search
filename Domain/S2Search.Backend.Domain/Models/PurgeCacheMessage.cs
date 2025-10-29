using System.Text.Json.Serialization;

namespace S2Search.Backend.Domain.Models;

public class PurgeCacheMessage
{
    [JsonPropertyName("host")]
    public string Host { get; set; }
}
