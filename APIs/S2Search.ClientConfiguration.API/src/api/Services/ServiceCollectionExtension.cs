using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces.Repositories;
using Services.Repositories;

namespace Services
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds all required dependencies for this API
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddAPIServices(this IServiceCollection services) =>
            services.AddRepositories();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddSingleton<ISearchIndexRepository, SearchIndexRepository>()
                    .AddSingleton<IThemeRepository, ThemeRepository>()
                    .AddSingleton<ISearchConfigurationRepository, SearchConfigurationRepository>();
    }
}
