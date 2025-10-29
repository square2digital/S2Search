using System;

namespace S2Search.Backend.Domain.AzureFunctions.FeedServices.Models
{
    public class GenericSynonym
    {
        public Guid Id { get; set; }
        public string SolrFormat { get; set; }
    }
}
