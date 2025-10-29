using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInstance;

public class SearchInstanceKey
{
    public Guid Id { get; set; }
    public Guid SearchInstanceId { get; set; }
    public string KeyType { get; set; }
    public string Name { get; set; }
    public string ApiKey { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsLatest { get; set; }
}
