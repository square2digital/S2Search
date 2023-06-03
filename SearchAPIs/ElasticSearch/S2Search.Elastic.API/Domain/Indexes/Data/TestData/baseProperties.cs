using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Indexes.Data.TestData
{
    public class BaseProperties
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        // ****************************
        // Generic Product Properties
        // ****************************

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("subtitle")]
        public string subtitle { get; set; }

        [JsonPropertyName("price")]
        public decimal price { get; set; }

        [JsonPropertyName("city")]
        public string city { get; set; }

        [JsonPropertyName("imageUrl")]
        public string imageUrl { get; set; }

        [JsonPropertyName("linkUrl")]
        public string linkUrl { get; set; }
    }
}
