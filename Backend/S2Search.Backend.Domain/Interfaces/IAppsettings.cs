using S2Search.Backend.Domain.Models;

namespace S2Search.Backend.Domain.Interfaces;

public interface IAppSettings
{
    bool UseGenericResponse { get; set; }
    bool Development { get; set; }

    DemoSearchCredentials DemoSearchCredentials { get; set; }
    SearchSettings SearchSettings { get; set; }
    FacetSettings FacetSettings { get; set; }
    ClientConfigurationSettings ClientConfigurationSettings { get; set; }
    MemoryCacheSettings MemoryCacheSettings { get; set; }
    RedisCacheSettings RedisCacheSettings { get; set; }
    AdminSettings AdminSettings { get; set; }
}
