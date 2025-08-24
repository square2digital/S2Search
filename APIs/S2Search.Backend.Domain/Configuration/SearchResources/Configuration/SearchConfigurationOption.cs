namespace S2Search.Backend.Domain.Configuration.SearchResources.Configuration;

public class SearchConfigurationOption
{
    public Guid SeachConfigurationOptionId { get; set; }
    public Guid SearchConfigurationMappingId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string FriendlyName { get; set; }
    public string Description { get; set; }
    public string DataType { get; set; }
    public int? OrderIndex { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}