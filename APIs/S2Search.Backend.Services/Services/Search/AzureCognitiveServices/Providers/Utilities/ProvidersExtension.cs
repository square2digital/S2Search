using Microsoft.Extensions.DependencyInjection;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.AzureSearch;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.Utilities
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
