using System;

namespace Domain.Models.Objects
{
    public class GenericSynonyms
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string SolrFormat { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
