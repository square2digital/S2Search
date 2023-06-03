namespace S2.Test.Importer
{
    public class AppSettings : IAppSettings
    {
        public SearchSettings SearchSettings { get; set; }
        public IndexSettings IndexSettings { get; set; }
        public APIKeys APIKeys { get; set; }
    }

    public class SearchSettings
    {
        public string SearchServiceEndpoint { get; set; }
        public string SearchServiceAPIKey { get; set; }
        public string SearchServiceName { get; set; }
        public string APIVersion { get; set; }
        public string DemoVehiclesURL { get; set; }
    }

    public class IndexSettings
    {
        public string SearchIndexName { get; set; }
        public string SearchIndexType { get; set; }
        public string MakesSynonymsMapName { get; set; }
    }

    public class APIKeys
    {
        public string PrimaryAdminKey { get; set; }
        public string SecondaryAdminKey { get; set; }
        public string QueryKey { get; set; }
    }
}
