using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Domain.Models.Response.Generic
{
    public class GenericResponse
    {
        // ****************
        // Base Properties
        // ****************

        [JsonPropertyName("id")]
        public string  Id { get; set; }

        // ****************************
        // Generic Product Properties
        // ****************************

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("linkUrl")]
        public string LinkUrl { get; set; }

        // *************************
        // Additional Property Types
        // *************************
        // 1 - Boolean
        // 2 - Text
        //      3a - Keyword - these will be facetable
        // 4 - Number
        //      4a - Int
        //      4b - Long
        //      4c - Decimal
        // 7 - Date
        // 8 - Geopoint <- maybe??
        //      8a - Decimal
        //      8b - Decimal

        [JsonPropertyName("additionalPropertiesBoolean")]
        public Dictionary<string, bool> AdditionalProperties_Boolean { get; set; }

        [JsonPropertyName("additionalPropertiesText")]
        public Dictionary<string, string> AdditionalProperties_Text { get; set; }

        [JsonPropertyName("additionalPropertiesKeyword")]
        public Dictionary<string, string> AdditionalProperties_Keyword { get; set; }

        [JsonPropertyName("additionalPropertiesInteger")]
        public Dictionary<string, int> AdditionalProperties_Integer { get; set; }

        [JsonPropertyName("additionalPropertiesLong")]
        public Dictionary<string, long> AdditionalProperties_Long { get; set; }

        [JsonPropertyName("additionalPropertiesDecimal")]
        public Dictionary<string, decimal> AdditionalProperties_Decimal { get; set; }

        [JsonPropertyName("additionalPropertiesDate")]
        public Dictionary<string, DateTime> AdditionalProperties_Date { get; set; }

        public GenericResponse()
        {
            AdditionalProperties_Boolean = new Dictionary<string, bool>();
            AdditionalProperties_Text = new Dictionary<string, string>();
            AdditionalProperties_Keyword = new Dictionary<string, string>();
            AdditionalProperties_Integer = new Dictionary<string, int>();
            AdditionalProperties_Long = new Dictionary<string, long>();
            AdditionalProperties_Decimal = new Dictionary<string, decimal>();
            AdditionalProperties_Date = new Dictionary<string, DateTime>();
        }
    }
}
