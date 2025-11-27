using LazyCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        public static IConfiguration? Configuration { get; set; }

        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            // Register LazyCache
            services.AddSingleton<IAppCache, CachingService>();


            var appSettings = LoadAppSettings(services);

            return services.AddRedis(appSettings.ConnectionStrings.Redis)
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
            services
                .AddSingleton(RedisConnectionMultiplexer())
                .AddSingleton<IDbContextProvider, DbContextProvider>()
                .AddSingleton<ISearchIndexRepository, SearchIndexRepository>()
                .AddSingleton<IMemoryCacheService, LazyCacheService>()
                .AddSingleton<IAzureSearchService, AzureSearchService>()
                .AddSingleton<IAzureFacetService, AzureFacetService>()
                .AddSingleton<IFacetHelper, FacetHelper>()
                .AddSingleton<IDisplayTextFormatHelper, DisplayTextFormatHelper>()
                .AddSingleton<ISearchFilterFormatter, SearchFilterFormatter>()
                .AddSingleton<IFacetOverride, EngineSizeOverride>()
                .AddSingleton<IFacetOverride, MileageOverride>()
                .AddSingleton<IFacetOverride, DoorsOverride>()
                .AddSingleton<IFacetOverrideProvider, FacetOverrideProvider>()
                .AddSingleton<ISynonymsService, SynonymsService>()
                .AddSingleton<IAzureQueueService, AzureQueueService>()
                .AddSingleton<IFireForgetService<IAzureQueueService>, FireForgetService<IAzureQueueService>>()
                .AddSingleton<IDbConnectionFactory, DbConnectionFactory>()
                .AddSingleton<IQueueClientProvider, QueueClientProvider>()

                .AddSingleton<IQueueManager, QueueManager>()

                .AddSingleton<IAzureSearchClientProvider, AzureSearchClientProvider>()
                .AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>()
                .AddSingleton<IDateTimeProvider, DateTimeProvider>()

                .AddSingleton<ISearchIndexRepository, SearchIndexRepository>()
                .AddSingleton<IFeedRepository, FeedRepository>()

                //.AddSingleton<ISearchInterfaceRepository, SearchInterfaceRepository>()
                .AddSingleton<ISynonymRepository, SynonymRepository>()
                .AddSingleton<ICustomerRepository, CustomerRepository>()
                .AddSingleton<IThemeRepository, ThemeRepository>()
                .AddSingleton<IFeedCredentialsRepository, FeedCredentialsRepository>()

                .AddSingleton<ISearchInsightsRetrievalManager, SearchInsightsRetrievalManager>()
                .AddSingleton<IPreviousDateRangeProvider, PreviousDateRangeProvider>()
                .AddSingleton<IPercentageChangeProvider, PercentageChangeProvider>()
                .AddSingleton<ISearchInsightFriendlyNameProvider, SearchInsightFriendlyNameProvider>()

                .AddSingleton<IFeedUploadManager, FeedUploadManager>()
                .AddSingleton<IFeedUploadValidationManager, FeedUploadValidationManager>()
                .AddSingleton<IFeedUploadDestinationManager, FeedUploadDestinationManager>()
                .AddSingleton<IFeedCredentialsManager, FeedCredentialsManager>()

                //.AddSingleton<ISearchConfigurationRepository, SearchConfigurationRepository>()
                .AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>()
                .AddSingleton<ISearchInsightsReportRepository, SearchInsightsReportRepository>()
                .AddSingleton<IFeedRepository, FeedRepository>()
                .AddSingleton<IBlobClientProvider, BlobClientProvider>()

                .AddSingleton<IFeedSettingsValidationManager, FeedSettingsValidationManager>()
                .AddSingleton<ICronDescriptionManager, CronDescriptionManager>()
                .AddSingleton<INotificationRuleValidationManager, NotificationRuleValidationManager>()
                .AddSingleton<ISearchInterfaceValidationManager, SearchInterfaceValidationManager>()
                .AddSingleton<ISolrFormatConversionManager, SolrFormatConversionManager>()
                .AddSingleton<ISynonymValidationManager, SynonymValidationManager>()
                .AddSingleton<IQueryKeyNameValidationManager, QueryKeyNameValidationManager>();

            //services.AddSingleton<ICacheManager, RedisCacheManager>();
            //.AddSingleton<INotificationRuleRepository, NotificationRuleRepository>()
            //services.AddSingleton<IPurgeCacheProcessor, PurgeCacheProcessor>();
            //.AddSingleton<INotificationRepository, NotificationRepository>()
            //.AddSingleton<IDashboardRepository, DashboardRepository>()
            //services.AddSingleton<IDashboardManager, DashboardManager>();
            //services.AddSingleton<IDateTimeCategoryProvider, DateTimeCategoryProvider>();
            //services.AddSingleton<ISearchInsightsManager, SearchInsightsManager>()
            //services.AddSingleton<ISearchFacetsFormatManager, SearchFacetsFormatManager>()
            //services.AddSingleton<IDataPointsExtractionManager, DataPointsExtractionManager>();

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

            var appSettings = Configuration.Get<AppSettings>();

            if (appSettings == null)
            {
                throw new InvalidOperationException("AppSettings section is missing or invalid in configuration.");
            }

            // Register the bound instance explicitly as IAppSettings so consumers get the configuration-bound object.
            services.AddSingleton<IAppSettings>(appSettings);

            return appSettings;
        }

        private static Func<IServiceProvider, IConnectionMultiplexer> RedisConnectionMultiplexer()
        {
            var redisConStr = Configuration.GetConnectionString(ConnectionStringKeys.Redis);

            return x =>
            {
                var loggerFactory = x.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(RedisConnectionMultiplexer));

                try
                {
                    var connection = ConnectionMultiplexer.Connect(redisConStr);
                    return connection;
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, $"Unable to connect to Redis using Connection String: '{redisConStr}'");
                    throw;
                }
            };
        }
    }
}
