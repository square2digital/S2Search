using Domain.Models;
using Domain.Models.Config;

namespace Domain.Interfaces
{
    public interface IAppSettings
    {
        bool UseGenericResponse { get; set; }
        bool Development { get; set; }

        ElasticSearchSettings ElasticSearchSettings { get; set; }
        DemoSearchCredentials DemoSearchCredentials { get; set; }
        SearchSettings SearchSettings { get; set; }
        FacetSettings FacetSettings { get; set; }
        ClientConfigurationSettings ClientConfigurationSettings { get; set; }
        MemoryCacheSettings MemoryCacheSettings { get; set; }
        RedisCacheSettings RedisCacheSettings { get; set; }
    }
}
