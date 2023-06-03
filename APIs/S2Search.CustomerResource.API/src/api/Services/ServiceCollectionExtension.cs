using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;
using Services.Managers;
using Services.Providers;
using Services.Repositories;

namespace Services
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds all required dependencies for this API including other package dependencies as well as Managers, Providers and Respositories
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAPIServices(this IServiceCollection services) =>
            services.AddServiceDependencies()
                    .AddManagers()
                    .AddProviders()
                    .AddRepositories();

        private static IServiceCollection AddServiceDependencies(this IServiceCollection services) =>
            services;

        private static IServiceCollection AddManagers(this IServiceCollection services) =>
            services
            .AddSingleton<IQueueManager, QueueManager>()
            .AddSingleton<IFeedSettingsValidationManager, FeedSettingsValidationManager>()
            .AddSingleton<ICronDescriptionManager, CronDescriptionManager>()
            .AddSingleton<IQueryKeyNameValidationManager, QueryKeyNameValidationManager>()
            .AddSingleton<INotificationRuleValidationManager, NotificationRuleValidationManager>()
            .AddSingleton<ISearchInterfaceValidationManager, SearchInterfaceValidationManager>()
            .AddSingleton<ISynonymValidationManager, SynonymValidationManager>()
            .AddSingleton<ISolrFormatConversionManager, SolrFormatConversionManager>()
            .AddSingleton<IFeedCredentialsManager, FeedCredentialsManager>()
            .AddSingleton<INotificationManager, NotificationManager>()
            .AddSingleton<IDashboardManager, DashboardManager>()
            .AddSingleton<IFeedUploadDestinationManager, FeedUploadDestinationManager>()
            .AddSingleton<IFeedUploadValidationManager, FeedUploadValidationManager>()
            .AddSingleton<IFeedUploadManager, FeedUploadManager>()
            .AddSingleton<ISearchInsightsRetrievalManager, SearchInsightsRetrievalManager>();

        private static IServiceCollection AddProviders(this IServiceCollection services) =>
            services
            .AddSingleton<IGuidProvider, GuidProvider>()
            .AddSingleton<IQueueClientProvider, QueueClientProvider>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IBlobClientProvider, BlobClientProvider>()
            .AddSingleton<IPreviousDateRangeProvider, PreviousDateRangeProvider>()
            .AddSingleton<IPercentageChangeProvider, PercentageChangeProvider>()
            .AddSingleton<ISearchInsightFriendlyNameProvider, SearchInsightFriendlyNameProvider>();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services
            .AddSingleton<ISearchIndexRepository, SearchIndexRepository>()
            .AddSingleton<IFeedRepository, FeedRepository>()
            .AddSingleton<INotificationRuleRepository, NotificationRuleRepository>()
            .AddSingleton<ISearchInterfaceRepository, SearchInterfaceRepository>()
            .AddSingleton<ISynonymRepository, SynonymRepository>()
            .AddSingleton<ICustomerRepository, CustomerRepository>()
            .AddSingleton<IThemeRepository, ThemeRepository>()
            .AddSingleton<IFeedCredentialsRepository, FeedCredentialsRepository>()
            .AddSingleton<INotificationRepository, NotificationRepository>()
            .AddSingleton<IDashboardRepository, DashboardRepository>()
            .AddSingleton<ISearchConfigurationRepository, SearchConfigurationRepository>()
            .AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>()
            .AddSingleton<ISearchInsightsReportRepository, SearchInsightsReportRepository>();
    }
}
