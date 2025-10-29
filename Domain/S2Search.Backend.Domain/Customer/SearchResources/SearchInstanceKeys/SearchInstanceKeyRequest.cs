using Domain.Customer.Enums;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInstanceKeys;

public class SearchInstanceKeyRequest
{
    public string Name { get; set; }
    public SearchInstanceKeyType Type { get; set; }
}
