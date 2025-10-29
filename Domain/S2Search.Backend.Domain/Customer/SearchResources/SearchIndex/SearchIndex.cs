using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

public class SearchIndex
{
    public Guid id { get; set; }
    public Guid? search_instance_id { get; set; }
    public Guid customer_id { get; set; }
    public string friendly_name { get; set; }
    public string index_name { get; set; }
    public string index_type { get; set; }
}
