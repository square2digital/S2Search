using Microsoft.Extensions.DependencyInjection;
using S2Search.Common.Database.Sql.Dapper.Utilities;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;
using Services.Managers;
using Services.Providers;
using Services.Repositories;

namespace Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFunctionServices(this IServiceCollection services) =>
            services.AddServiceDependencies()
                    .AddRepositories()
                    .AddProviders()
                    .AddManagers();

        private static IServiceCollection AddServiceDependencies(this IServiceCollection services) =>
            services.AddSqlDapperAbstractions();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>();

        private static IServiceCollection AddProviders(this IServiceCollection services) =>
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>()
                    .AddSingleton<IDateTimeCategoryProvider, DateTimeCategoryProvider>();

        private static IServiceCollection AddManagers(this IServiceCollection services) =>
            services.AddSingleton<ISearchInsightsManager, SearchInsightsManager>()
                    .AddSingleton<ISearchFacetsFormatManager, SearchFacetsFormatManager>()
                    .AddSingleton<IDataPointsExtractionManager, DataPointsExtractionManager>();
    }
}
