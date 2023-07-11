using Microsoft.Extensions.DependencyInjection;
using S2Search.Common.Providers.AzureSearch;

namespace S2Search.Common.Providers.Utilities
{
    public static class ProvidersExtension
    {
        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<IAzureSearchClientProvider, AzureSearchClientProvider>();
            return services;
        }
    }
}
