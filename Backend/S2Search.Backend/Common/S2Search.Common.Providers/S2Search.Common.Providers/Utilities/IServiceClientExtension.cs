using Microsoft.Extensions.DependencyInjection;

namespace S2Search.Backend.Common.S2Search.Common.Providers.S2Search.Common.Providers.Utilities
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddS2SearchProviders(this IServiceCollection services)
        {
            services.AddLazyCache();
            services.AddProviders();
            return services;
        }
    }
}
