using LazyCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; // Add this for configuration binding extensions
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Interfaces.FacetOverrides;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Domain.Models;
using S2Search.Backend.Services.Admin.Configuration.Repositories;
using S2Search.Backend.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Admin.Customer.Managers;
using S2Search.Backend.Services.Services;
using S2Search.Backend.Services.Services.Admin.Configuration.Repositories;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Admin.Customer.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Repositories;
using S2Search.Backend.Services.Services.Admin.Dapper.Providers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers.FacetOverrides;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.AzureSearch;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services.Cache;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces;
using Services.Providers;
using Services.Services;
using StackExchange.Redis;

namespace S2Search.Backend.Services
{
    public static class ServiceCollectionExtension
    {
        // Make Configuration nullable and settable
        public static IConfiguration? Configuration { get; set; }

        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            // Register LazyCache
            services.AddSingleton<IAppCache, CachingService>();

            // Register your IAppSettings implementation
            services.AddSingleton<IAppSettings, AppSettings>();

            var appSettings = LoadAppSettings(services);

            return services.AddRedis(appSettings.RedisCacheSettings.RedisConnectionString)
                            .AddServiceDependencies()
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
            //services.AddLazyCache();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbContextProvider, DbContextProvider>();
            services.AddSingleton<ISearchIndexRepository, SearchIndexRepository>();
            services.AddSingleton<IMemoryCacheService, LazyCacheService>();
            services.AddSingleton<IAzureSearchService, AzureSearchService>();
            services.AddSingleton<IAzureFacetService, AzureFacetService>();
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
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton(RedisConnectionMultiplexer(Configuration));
            services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
            //services.AddSingleton<ICacheManager, RedisCacheManager>();
            services.AddSingleton<IQueueManager, QueueManager>();
            //services.AddSingleton<IPurgeCacheProcessor, PurgeCacheProcessor>();
            services.AddSingleton<IAzureSearchClientProvider, AzureSearchClientProvider>();
            services.AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            //services.AddSingleton<IDashboardManager, DashboardManager>();
            //services.AddSingleton<IDateTimeCategoryProvider, DateTimeCategoryProvider>();
            //services.AddSingleton<ISearchInsightsManager, SearchInsightsManager>()
            //services.AddSingleton<ISearchFacetsFormatManager, SearchFacetsFormatManager>()
            //services.AddSingleton<IDataPointsExtractionManager, DataPointsExtractionManager>();

            services
                .AddSingleton<ISearchIndexRepository, SearchIndexRepository>()
                .AddSingleton<IFeedRepository, FeedRepository>()
                //.AddSingleton<INotificationRuleRepository, NotificationRuleRepository>()
                .AddSingleton<ISearchInterfaceRepository, SearchInterfaceRepository>()
                .AddSingleton<ISynonymRepository, SynonymRepository>()
                .AddSingleton<ICustomerRepository, CustomerRepository>()
                .AddSingleton<IThemeRepository, ThemeRepository>()
                .AddSingleton<IFeedCredentialsRepository, FeedCredentialsRepository>()
                //.AddSingleton<INotificationRepository, NotificationRepository>()
                //.AddSingleton<IDashboardRepository, DashboardRepository>()
                .AddSingleton<ISearchConfigurationRepository, SearchConfigurationRepository>()
                .AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>()
                .AddSingleton<ISearchInsightsReportRepository, SearchInsightsReportRepository>()
                .AddSingleton<IFeedRepository, FeedRepository>();

            services.AddSingleton<IFeedSettingsValidationManager, FeedSettingsValidationManager>();
            services.AddSingleton<ICronDescriptionManager, CronDescriptionManager>();
            services.AddSingleton<INotificationRuleValidationManager, NotificationRuleValidationManager>();
            services.AddSingleton<ISearchInterfaceValidationManager, SearchInterfaceValidationManager>();
            services.AddSingleton<ISolrFormatConversionManager, SolrFormatConversionManager>();
            services.AddSingleton<ISynonymValidationManager, SynonymValidationManager>();


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
            return services;
        }

        private static IAppSettings LoadAppSettings(IServiceCollection services)
        {
            if (Configuration == null)
            {
                throw new InvalidOperationException("Configuration must be set before calling AddAPIServices.");
            }
                
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings == null)
            {
                throw new InvalidOperationException("AppSettings section is missing or invalid in configuration.");
            }                

            services.AddSingleton(appSettings);

            return appSettings;
        }

        private static Func<IServiceProvider, IConnectionMultiplexer> RedisConnectionMultiplexer(IConfiguration configuration)
        {
            return x =>
            {
                var loggerFactory = x.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(RedisConnectionMultiplexer));

                try
                {
                    var connection = ConnectionMultiplexer.Connect(configuration.GetValue<string>(ConnectionStrings.Redis));
                    return connection;
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, $"Unable to connect to Redis using Configuration Key: '{ConnectionStrings.Redis}'");
                    throw;
                }
            };
        }
    }
}
