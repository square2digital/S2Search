using Microsoft.Extensions.DependencyInjection;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Providers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.Dapper;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Utilities
{
    public static class ServiceClientExtension
    {
        public static IServiceCollection AddSqlDapperAbstractions(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDbContextProvider, DbContextProvider>();
            return services;
        }
    }
}
