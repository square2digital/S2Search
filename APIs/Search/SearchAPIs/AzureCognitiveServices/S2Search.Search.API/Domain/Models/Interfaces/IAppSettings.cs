namespace Domain.Models.Interfaces
{
    public interface IAppSettings
    {
        DemoSearchCredentials DemoSearchCredentials { get; set; }
        SearchSettings SearchSettings { get; set; }
        FacetSettings FacetSettings { get; set; }
        ClientConfigurationSettings ClientConfigurationSettings { get; set; }
        MemoryCacheSettings MemoryCacheSettings { get; set; }
        RedisCacheSettings RedisCacheSettings { get; set; }
        AdminSettings AdminSettings { get; set; }
    }
}