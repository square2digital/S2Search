using System;

namespace S2Search.Backend.Domain.Configuration.SearchResources.Synonyms;

public class GenericSynonyms
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public string SolrFormat { get; set; }
    public DateTime CreatedDate { get; set; }
}
