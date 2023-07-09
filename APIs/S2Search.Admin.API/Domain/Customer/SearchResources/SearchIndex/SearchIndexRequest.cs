using Domain.Customer.SearchResources.SearchInstance;

namespace Domain.Customer.SearchResources.SearchIndex
{
    public class SearchIndexRequest
    {
        public Guid CustomerId { get; set; }
        public string IndexName { get; set; }
        public string IndexType { get; set; }
        public SearchInstanceRequest Configuration { get; set; }
    }
}