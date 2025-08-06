namespace S2Search.Backend.Common.S2Search.Common.Models.S2Search.Common.Models.Settings
{
    public class AppSettings
    {
        public SearchSettings SearchSettings { get; set; }
        public IndexSettings IndexSettings { get; set; }
        public APIKeys Keys { get; set; }
    }

    public class SearchSettings
    {
        public string SearchServiceName { get; set; }
        public string DataSourceURL { get; set; }
    }

    public class IndexSettings
    {
        public string SearchIndexName { get; set; }
        public string MakesSynonymsMapName { get; set; }
    }

    public class APIKeys
    {
        public string PrimaryAdminKey { get; set; }
        public string SecondaryAdminKey { get; set; }
        public string QueryKey { get; set; }
    }
}
