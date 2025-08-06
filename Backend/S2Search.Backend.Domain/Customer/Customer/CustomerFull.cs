using S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

namespace S2Search.Backend.Domain.Customer.Customer
{
    public class CustomerFull
    {
        public CustomerIds Customer { get; set; }
        public IEnumerable<SearchIndex> SearchIndexes { get; set; }
    }
}
