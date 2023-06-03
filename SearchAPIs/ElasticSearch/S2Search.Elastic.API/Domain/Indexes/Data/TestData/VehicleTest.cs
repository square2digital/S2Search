using System.Text.Json.Serialization;

namespace Domain.Indexes.Data.TestData
{
    public class VehicleTest : BaseProperties
    {
        [JsonPropertyName("vehicleID")]
        public string vehicleID { get; set; }

        [JsonPropertyName("vrm")]
        public string vrm { get; set; }

        [JsonPropertyName("imageURL")]
        public string imageURL { get; set; }

        [JsonPropertyName("pageUrl")]
        public string pageUrl { get; set; }

        [JsonPropertyName("make")]
        public string make { get; set; }

        [JsonPropertyName("model")]
        public string model { get; set; }

        [JsonPropertyName("variant")]
        public string variant { get; set; }

        [JsonPropertyName("city")]
        public string city { get; set; }

        [JsonPropertyName("price")]
        public int price { get; set; }

        [JsonPropertyName("monthlyPrice")]
        public int monthlyPrice { get; set; }

        [JsonPropertyName("mileage")]
        public int mileage { get; set; }

        [JsonPropertyName("fuelType")]
        public string fuelType { get; set; }

        [JsonPropertyName("transmission")]
        public string transmission { get; set; }

        [JsonPropertyName("doors")]
        public int doors { get; set; }

        [JsonPropertyName("engineSize")]
        public int engineSize { get; set; }

        [JsonPropertyName("bodyStyle")]
        public string bodyStyle { get; set; }

        [JsonPropertyName("colour")]
        public string colour { get; set; }

        [JsonPropertyName("manufactureColour")]
        public object manufactureColour { get; set; }

        [JsonPropertyName("year")]
        public int year { get; set; }

        [JsonPropertyName("modelYear")]
        public string modelYear { get; set; }
    }
}
