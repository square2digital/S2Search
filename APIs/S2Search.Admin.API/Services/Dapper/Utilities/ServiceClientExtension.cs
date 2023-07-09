using Microsoft.Extensions.DependencyInjection;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Dapper.Providers;

namespace Services.Dapper.Utilities
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
