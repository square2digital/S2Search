using Domain.Customer.Shared;

namespace Domain.Customer.SearchResources.SearchIndex
{
    public class SearchIndexKeyDeletionRequest
    {
        public Guid SearchIndexId { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<QueryKey> KeysToDelete { get; set; }
    }
}
