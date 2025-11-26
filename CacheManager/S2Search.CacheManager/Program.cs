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
        static async Task<int> Main(string[] args)
        {
            using (var host = CreateHostBuilder(args).Build())
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Cache Manager Starting up...");

                // shutdown via Ctrl+C
                using (var cts = new CancellationTokenSource())
                {
                    Console.CancelKeyPress += (s, e) =>
                    {
                        e.Cancel = true;
                        logger.LogInformation("Cancel requested via Console. Shutting down...");
                        cts.Cancel();
                    };

                    try
                    {
                        var processor = host.Services.GetRequiredService<IPurgeCacheProcessor>();
                        await processor.RunAsync(cts.Token);
                        return 0;
                    }
                    catch (OperationCanceledException)
                    {
                        logger.LogInformation("Shutdown requested.");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical(ex, "Unhandled exception in Cache Manager.");
                        return 1;
                    }
                    finally
                    {
                        logger.LogInformation("Cache Manager Shutting Down...");
                    }
                }
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging();
                    services.Configure<AppSettings>(context.Configuration);
                    services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

                    // Register Redis connection multiplexer as a singleton and let DI dispose it on shutdown.
                    services.AddSingleton<IConnectionMultiplexer>(sp =>
                    {
                        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("RedisConnectionMultiplexer");

                        var redisConStr = context.Configuration.GetConnectionString(ConnectionStringKeys.Redis);
                        if (string.IsNullOrWhiteSpace(redisConStr))
                        {
                            logger.LogCritical("Redis connection string is not configured. Key: {Key}", ConnectionStringKeys.Redis);
                            throw new InvalidOperationException("Redis connection string is missing.");
                        }

                        try
                        {
                            var connection = ConnectionMultiplexer.Connect(redisConStr);
                            logger.LogInformation("Connected to Redis.");
                            return connection;
                        }
                        catch (Exception ex)
                        {
                            logger.LogCritical(ex, "Unable to connect to Redis using Connection String: '{ConnStr}'", redisConStr);
                            throw;
                        }
                    });

                    services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
                    services.AddSingleton<ICacheManager, RedisCacheManager>();
                    services.AddSingleton<IQueueManager, QueueManager>();
                    services.AddSingleton<IPurgeCacheProcessor, PurgeCacheProcessor>();
                });
        }
    }
}