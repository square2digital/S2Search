using S2Search.Backend.Domain.Customer.Shared;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInstanceKeys;

public class SearchInstanceQueryKeyDeletionRequest
{
    public Guid SearchIndexId { get; set; }
    public Guid SearchInstanceId { get; set; }
    public IEnumerable<QueryKey> KeysToDelete { get; set; }
}
