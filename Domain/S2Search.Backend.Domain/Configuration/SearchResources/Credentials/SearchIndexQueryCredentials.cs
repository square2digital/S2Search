using System;

namespace S2Search.Backend.Domain.Configuration.SearchResources.Credentials;

public class SearchIndexQueryCredentials
{
    public Guid id { get; set; }
    public string QueryApiKey { get; set; }
    public string search_index_name { get; set; }
    public string search_instance_name { get; set; }
    public string search_instance_endpoint { get; set; }
    public string api_key { get; set; }
}