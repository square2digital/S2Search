using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace S2Search.Backend.Services
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services, IConfiguration configuration)
        {
            ServiceCollectionExtension.Configuration = configuration;
            return services.AddAPIServices();
        }
    }
}