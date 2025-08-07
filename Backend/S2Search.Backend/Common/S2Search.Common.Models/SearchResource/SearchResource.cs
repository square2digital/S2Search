using System;

namespace S2Search.Backend.Common.S2Search.Common.Models.SearchResource
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
