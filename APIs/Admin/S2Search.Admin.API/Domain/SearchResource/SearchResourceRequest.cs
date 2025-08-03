using Domain.ServiceResource;

namespace Domain.SearchResource
{
    public class SearchResourceRequest
    {
        public Guid CustomerId { get; set; }
        public string IndexName { get; set; }
        public string IndexType { get; set; }
        public ServiceResourceRequest Configuration { get; set; }
    }
}
