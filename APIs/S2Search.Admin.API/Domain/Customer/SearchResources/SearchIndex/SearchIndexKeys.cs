using System;

namespace Domain.Customer.SearchResources.SearchIndex
{
    public class SearchIndexKeys
    {
        public string Name { get; set; }
        public Guid ApiKey { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
