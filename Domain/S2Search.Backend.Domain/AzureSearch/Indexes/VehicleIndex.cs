using Azure.Search.Documents.Indexes;
using System.Text.Json.Serialization;

namespace S2Search.Backend.Domain.AzureSearch.Indexes;

public partial class VehicleIndex
{
    [SimpleField(IsKey = true)]
    [JsonPropertyName("vehicleID")]
    public string VehicleID { get; set; }

    [JsonPropertyName("vRM")]
    public string VRM { get; set; }

    [JsonPropertyName("imageURL")]
    public string ImageURL { get; set; }

    [JsonPropertyName("pageUrl")]
    public string PageUrl { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("make")]
    public string Make { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("variant")]
    public string Variant { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("location")]
    public string Location { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("price")]
    public int Price { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("monthlyPrice")]
    public double MonthlyPrice { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("mileage")]
    public int Mileage { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("fuelType")]
    public string FuelType { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("transmission")]
    public string Transmission { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("doors")]
    public int Doors { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("engineSize")]
    public int EngineSize { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("bodyStyle")]
    public string BodyStyle { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("colour")]
    public string Colour { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("manufactureColour")]
    public string ManufactureColour { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [SearchableField(IsFilterable = false, IsSortable = false, IsFacetable = false)]
    [JsonPropertyName("modelYear")]
    public string ModelYear { get; set; }

    [SearchableField(IsHidden = true)]
    [JsonPropertyName("autocompleteSuggestion")]
    public string AutocompleteSuggestion
    {
        get
        {
            return $"{Make} {Model}";
        }
    }
}
