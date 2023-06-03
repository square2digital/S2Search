using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class PurgeCacheMessage
    {
        [JsonPropertyName("host")]
        public string Host { get; set; }
    }
}
