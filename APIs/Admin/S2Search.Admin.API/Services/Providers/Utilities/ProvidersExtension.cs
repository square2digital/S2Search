using Microsoft.Extensions.DependencyInjection;
using Services.Providers.AzureSearch;

namespace Services.Providers.Utilities
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
