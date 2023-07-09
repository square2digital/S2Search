using Domain.Customer.Shared;

namespace Domain.Customer.SearchResources.SearchInstanceKeys
{
    public class SearchInstanceQueryKeyDeletionRequest
    {
        public Guid SearchIndexId { get; set; }
        public Guid SearchInstanceId { get; set; }
        public IEnumerable<QueryKey> KeysToDelete { get; set; }
    }
}
