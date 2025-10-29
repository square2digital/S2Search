namespace S2Search.Backend.Domain.Configuration.SearchResources.Configuration;

public class SearchConfigurationOption
{
    public Guid SeachConfigurationOptionId { get; set; }
    public Guid SearchConfigurationMappingId { get; set; }
    public string key { get; set; }
    public string value { get; set; }
    public string friendly_name { get; set; }
    public string description { get; set; }
    public string data_type { get; set; }
    public int? order_index { get; set; }
    public DateTime created_date { get; set; }
    public DateTime modified_date { get; set; }
}