using System;

namespace Domain.SearchResource
{
    public class SearchResource
    {
        public Guid SearchResourceId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? ServiceResourceId { get; set; }
        public string Name { get; set; }
        public string IndexName { get; set; }
        public string IndexType { get; set; }
    }
}
