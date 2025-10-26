using System;

namespace S2Search.Backend.Domain.Configuration.SearchResources.Synonyms;

public class GenericSynonyms
{
    public Guid id { get; set; }
    public string category { get; set; }
    public string solr_format { get; set; }
    public DateTime created_date { get; set; }
}
