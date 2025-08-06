using Microsoft.Extensions.DependencyInjection;

namespace S2Search.Backend.Services.Services.Admin.Utilities
{
    public static class ServiceClientExtension
    {
        public static IServiceCollection AddSqlDapperAbstractions(this IServiceCollection services)
        {
            return services;
        }
    }
}
