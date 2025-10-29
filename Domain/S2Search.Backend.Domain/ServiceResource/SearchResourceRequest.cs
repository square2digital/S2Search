using S2Search.Backend.Domain.SearchResource;

namespace S2Search.Backend.Common.S2Search.Common.Models.SearchResource;

public class SearchResourceRequest
{
    public Guid CustomerId { get; set; }
    public string IndexName { get; set; }
    public string IndexType { get; set; }
    public ServiceResourceRequest Configuration { get; set; }
}
