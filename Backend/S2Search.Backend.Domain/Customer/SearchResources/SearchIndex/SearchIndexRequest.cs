using S2Search.Backend.Domain.Customer.SearchResources.SearchInstance;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

public class SearchIndexRequest
{
    public Guid CustomerId { get; set; }
    public string IndexName { get; set; }
    public string IndexType { get; set; }
    public SearchInstanceRequest Configuration { get; set; }
}