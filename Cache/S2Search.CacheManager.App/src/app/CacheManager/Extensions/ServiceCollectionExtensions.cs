using Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Providers;
using Services.Interfaces.Managers;
using Services.Interfaces.Processors;
using Services.Interfaces.Providers;
using Services.Managers;
using Services.Processors;
using Services.Providers;
using StackExchange.Redis;

namespace CacheManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCacheManagerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton(RedisConnectionMultiplexer(configuration));
            services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
            services.AddSingleton<ICacheManager, RedisCacheManager>();
            services.AddSingleton<IQueueManager, QueueManager>();
            services.AddSingleton<IPurgeCacheProcessor, PurgeCacheProcessor>();
            return services;
        }

        private static Func<IServiceProvider, IConnectionMultiplexer> RedisConnectionMultiplexer(IConfiguration configuration)
        {
            return x =>
            {
                var loggerFactory = x.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(RedisConnectionMultiplexer));

                try
                {
                    var connection = ConnectionMultiplexer.Connect(configuration.GetValue<string>(ConnectionStrings.Redis));
                    return connection;
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, $"Unable to connect to Redis using Configuration Key: '{ConnectionStrings.Redis}'");
                    throw;
                }
            };
        }
    }
}
