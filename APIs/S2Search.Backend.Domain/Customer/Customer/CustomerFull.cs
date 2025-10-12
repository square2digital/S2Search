using S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

namespace S2Search.Backend.Domain.Customer.Customer;

public class CustomerFull
{
    public CustomerIds customer { get; set; }
    public IEnumerable<SearchIndex> search_indexes { get; set; }
}
