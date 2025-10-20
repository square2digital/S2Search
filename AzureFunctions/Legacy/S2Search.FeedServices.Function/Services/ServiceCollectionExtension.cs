using Microsoft.Extensions.DependencyInjection;
using S2Search.Common.Database.Sql.Dapper.Utilities;
using Services.Interfaces.Managers;
using Services.Interfaces.Mappers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;
using Services.Managers;
using Services.Mappers.FeedTypes;
using Services.Providers;
using Services.Repositories;

namespace Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFunctionServices(this IServiceCollection services) =>
            services.AddServiceDependencies()
                    .AddManagers()
                    .AddProviders()
                    .AddRepositories()
                    .AddMappers();

        private static IServiceCollection AddServiceDependencies(this IServiceCollection services) =>
            services
            .AddSqlDapperAbstractions()
            .AddLazyCache();

        private static IServiceCollection AddManagers(this IServiceCollection services) =>
            services
            .AddSingleton<IFeedTargetQueueManager, FeedTargetQueueManager>()
            .AddSingleton<IZipArchiveValidationManager, ZipArchiveValidationManager>()
            .AddSingleton<IFeedProcessingManager, FeedProcessingManager>();

        private static IServiceCollection AddProviders(this IServiceCollection services) =>
            services
            .AddSingleton<IAzureSearchDocumentsClientProvider, AzureSearchDocumentsClientProvider>()
            .AddSingleton<IAzureSearchManager, AzureSearchManager>()
            .AddSingleton<ICsvParserManager, TinyCsvParserManager>()
            .AddSingleton<IFeedMapperProvider, FeedMapperProvider>();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services
            .AddSingleton<IFeedRepository, FeedRepository>()
            .AddSingleton<ISearchIndexCredentialsRepository, SearchIndexCredentialsRepository>()
            .AddSingleton<IGenericSynonymRepository, GenericSynonymRepository>();

        private static IServiceCollection AddMappers(this IServiceCollection services) =>
            services
            .AddSingleton<IFeedMapper, DefaultFeedMapper>()
            .AddSingleton<IFeedMapper, DMS14FeedMapper>();
    }
}
