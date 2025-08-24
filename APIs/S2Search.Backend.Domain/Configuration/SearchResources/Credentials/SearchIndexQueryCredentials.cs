using System;

namespace S2Search.Backend.Domain.Configuration.SearchResources.Credentials;

public class SearchIndexQueryCredentials
{
    public Guid SearchIndexId { get; set; }
    public string QueryApiKey { get; set; }
    public string SearchIndexName { get; set; }
    public string SearchInstanceName { get; set; }
    public string SearchInstanceEndpoint { get; set; }
    public string ApiKey { get; set; }
}
