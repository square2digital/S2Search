using Microsoft.Extensions.DependencyInjection;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Interfaces.FacetOverrides;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.AzureSearch;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.Credentials;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services.Cache;
using S2Search.Backend.Services.Services.Search.Elastic.Helpers;
using S2Search.Backend.Services.Services.Search.Elastic.Helpers.FacetOverrides;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Providers;
using Services.Services;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices
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
            services.AddHttpClient<IS2SearchAPIClient, S2SearchAPIClient>();            
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
            services.AddSingleton<ISearchFilterFormatter, SearchFilterFormatterElastic>();
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
