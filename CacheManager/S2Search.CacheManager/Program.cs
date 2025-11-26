using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Providers;
using Services.Interfaces.Managers;
using Services.Interfaces.Processors;
using Services.Managers;
using Services.Processors;
using StackExchange.Redis;

namespace CacheManagerApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Cache Manager Starting up...");

            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = CreateHostBuilder(args);

            builder.ConfigureServices(services =>
            {
                services.AddLogging();

                services.Configure<AppSettings>(configuration);
                services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

                services.AddSingleton(RedisConnectionMultiplexer(configuration));
                services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
                services.AddSingleton<ICacheManager, RedisCacheManager>();
                services.AddSingleton<IQueueManager, QueueManager>();
                services.AddSingleton<IPurgeCacheProcessor, PurgeCacheProcessor>();
            });

            var host = builder.Build();

            var cancelTokenSource = new CancellationTokenSource();

            using (host)
            {
                try
                {
                    await host.Services.GetRequiredService<IPurgeCacheProcessor>().RunAsync(cancelTokenSource.Token);
                }
                catch(Exception ex) 
                {
                    Console.WriteLine($"Exception caught {ex}");
                }
                finally
                {
                    cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
                    await host.WaitForShutdownAsync(cancelTokenSource.Token);
                }
            }

            Console.WriteLine("Cache Manager Shutting Down...");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }

        private static Func<IServiceProvider, IConnectionMultiplexer> RedisConnectionMultiplexer(IConfiguration configuration)
        {
            var redisConStr = configuration.GetConnectionString(ConnectionStringKeys.Redis);

            return x =>
            {
                var loggerFactory = x.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(RedisConnectionMultiplexer));

                try
                {
                    var connection = ConnectionMultiplexer.Connect(redisConStr);
                    return connection;
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, $"Unable to connect to Redis using Connection String: '{redisConStr}'");
                    throw;
                }
            };
        }
    }
}