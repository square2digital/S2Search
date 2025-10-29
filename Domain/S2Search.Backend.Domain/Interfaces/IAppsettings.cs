using S2Search.Backend.Domain.Models;

namespace S2Search.Backend.Domain.Interfaces;

public interface IAppSettings
{
    DemoSearchCredentials DemoSearchCredentials { get; set; }
    SearchSettings SearchSettings { get; set; }
    FacetSettings FacetSettings { get; set; }
    MemoryCacheSettings MemoryCacheSettings { get; set; }
    RedisCacheSettings RedisCacheSettings { get; set; }
    ConnectionStrings ConnectionStrings { get; set; }
}
