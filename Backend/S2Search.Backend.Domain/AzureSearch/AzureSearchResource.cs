namespace S2Search.Backend.Domain.AzureSearch
{
    public class AzureSearchResource
    {
        public string SearchServiceEndpoint { get; set; }
        public string SearchServiceAPIKey { get; set; }
        public string SearchServiceName { get; set; }
        public string SearchIndexName { get; set; }
        public string AdminApiKey { get; set; }
        public string SearchIndexType { get; set; }
    }
}
