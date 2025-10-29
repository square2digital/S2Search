namespace S2.Test.Importer
{
    public interface IAppSettings
    {
        SearchSettings SearchSettings { get; set; }
        IndexSettings IndexSettings { get; set; }
        APIKeys APIKeys { get; set; }
    }
}