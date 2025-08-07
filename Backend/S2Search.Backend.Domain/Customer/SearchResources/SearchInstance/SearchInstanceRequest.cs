namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInstance;

public class SearchInstanceRequest
{
    public string Location { get; set; }
    public string PricingTier { get; set; }
    public bool IsShared { get; set; }
}
