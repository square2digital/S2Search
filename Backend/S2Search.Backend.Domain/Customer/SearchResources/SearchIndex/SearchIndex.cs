using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchIndex
{
    public class SearchIndex
    {
        public Guid SearchIndexId { get; set; }
        public Guid? SearchInstanceId { get; set; }
        public Guid CustomerId { get; set; }
        public string FriendlyName { get; set; }
        public string IndexName { get; set; }
        public string IndexType { get; set; }
    }
}
