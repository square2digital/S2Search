using Domain.SearchResources.SearchInstance;
using System;

namespace Domain.SearchResources.SearchIndex
{
    public class SearchIndexRequest
    {
        public Guid CustomerId { get; set; }
        public string IndexName { get; set; }
        public string IndexType { get; set; }
        public SearchInstanceRequest Configuration { get; set; }
    }
}