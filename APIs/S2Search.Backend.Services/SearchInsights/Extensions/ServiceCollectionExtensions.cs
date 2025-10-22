using Microsoft.Extensions.DependencyInjection;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Managers;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Providers;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Repositories;
using S2Search.Backend.Services.SearchInsights.Managers;
using S2Search.Backend.Services.SearchInsights.Providers;
using S2Search.Backend.Services.SearchInsights.Repositories;
using S2Search.Backend.Services.Services.Admin.Dapper.Utilities;

namespace S2Search.Backend.Services.SearchInsights.Extensions
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
