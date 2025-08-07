using Microsoft.Extensions.DependencyInjection;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Dapper.Providers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Dapper.Utilities
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
