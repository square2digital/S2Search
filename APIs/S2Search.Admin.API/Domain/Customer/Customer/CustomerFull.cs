using Domain.Customer.SearchResources.SearchIndex;

namespace Domain.Customer.Customer
{
    public class CustomerFull
    {
        public CustomerIds Customer { get; set; }
        public IEnumerable<SearchIndex> SearchIndexes { get; set; }
    }
}
