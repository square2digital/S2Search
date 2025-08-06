using Microsoft.Extensions.DependencyInjection;
using Services.Providers.AzureSearch;

namespace S2Search.Backend.Services.Services.Admin.Providers.Utilities
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
