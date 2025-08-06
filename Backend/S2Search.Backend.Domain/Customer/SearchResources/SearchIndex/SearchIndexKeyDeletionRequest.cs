using S2Search.Backend.Domain.Customer.Shared;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchIndex
{
    public class SearchIndexKeyDeletionRequest
    {
        public Guid SearchIndexId { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<QueryKey> KeysToDelete { get; set; }
    }
}
