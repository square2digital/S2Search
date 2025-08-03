using Microsoft.Extensions.DependencyInjection;

namespace S2Search.Common.Database.Sql.Dapper.Utilities
{
    public static class ServiceClientExtension
    {
        public static IServiceCollection AddSqlDapperAbstractions(this IServiceCollection services)
        {
            return services;
        }
    }
}
