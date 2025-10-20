using Microsoft.Extensions.DependencyInjection;
using S2Search.Common.Database.Sql.Dapper.Utilities;
using S2Search.SFTPGo.Client.AutoRest;
using Services.Extensions;
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
        public static IServiceCollection AddFunctionServices(this IServiceCollection services)
        {
            services.AddServiceDependencies()
                    .AddManagers()
                    .AddProviders()
                    .AddRepositories();

            return services;
        }

        private static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddHttpClient<ISFTPGoClient, SFTPGoClientExtended>();
            services.AddSqlDapperAbstractions();
            return services;
        }

        private static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHashManager, PasswordHashManager>();
            services.AddSingleton<ISFTPGoUserManager, SFTPGoUserManager>();
            return services;
        }

        private static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<IBasicAuthProvider, BasicAuthProvider>();
            services.AddHttpClient<IAccessTokenProvider, AccessTokenProvider>().ConfigureHttpClient((provider, httpClient) =>
            {
                var authToken = provider.GetRequiredService<IBasicAuthProvider>().GetAuthToken();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IFeedCredentialsRepository, FeedCredentialsRepository>();
            return services;
        }
    }
}
