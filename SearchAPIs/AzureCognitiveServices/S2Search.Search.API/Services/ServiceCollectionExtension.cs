using Microsoft.Extensions.DependencyInjection;
using S2Search.ClientConfigurationApi.Client.AutoRest;
using Services.Extensions;
using Services.Helper;
using Services.Helpers;
using Services.Helpers.FacetOverrides;
using Services.Interfaces;
using Services.Interfaces.Cache;
using Services.Interfaces.FacetOverrides;
using Services.Providers;
using Services.Providers.AzureSearch;
using Services.Services;
using Services.Services.Cache;

namespace Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            return services.AddServiceDependencies()
                           .AddServices()
                           .AddProviders();
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDistributedCacheService>(x => new RedisService(connectionString));
            return services;
        }

        private static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddLazyCache();
            services.AddHttpClient<IClientConfigurationApiClient, ClientConfigurationApiClientExtended>();            
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMemoryCacheService, LazyCacheService>();
            services.AddSingleton<IAzureSearchService, AzureSearchService>();
            services.AddSingleton<IAzureFacetService, AzureFacetService>();
            services.AddTransient<ILuceneSyntaxHelper, LuceneSyntaxHelper>();
            services.AddSingleton<IFacetHelper, FacetHelper>();
            services.AddSingleton<IDisplayTextFormatHelper, DisplayTextFormatHelper>();
            services.AddSingleton<ISearchFilterFormatter, SearchFilterFormatter>();
            services.AddSingleton<IFacetOverride, EngineSizeOverride>();
            services.AddSingleton<IFacetOverride, MileageOverride>();
            services.AddSingleton<IFacetOverride, DoorsOverride>();
            services.AddSingleton<IFacetOverrideProvider, FacetOverrideProvider>();
            services.AddSingleton<ISynonymsService, SynonymsService>();            
            services.AddSingleton<IAzureQueueService, AzureQueueService>();
            services.AddSingleton<IFireForgetService<IAzureQueueService>, FireForgetService<IAzureQueueService>>();
            return services;
        }

        private static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<ISearchIndexQueryCredentialsProvider, SearchIndexQueryCredentialsProvider>(); 
            services.AddSingleton<IAzureAutoSuggestOptionsProvider, AzureAutoSuggestOptionsProvider>();
            services.AddSingleton<IAzureSearchDocumentsClientProvider, AzureSearchDocumentsClientProvider>();
            services.AddSingleton<ISearchOptionsProvider, SearchOptionsProvider>();
            services.AddSingleton<ISynonymsHelper, SynonymsHelper>();
            services.AddSingleton<IAzureQueueClientProvider, AzureQueueClientProvider>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            return services;
        }
    }
}
