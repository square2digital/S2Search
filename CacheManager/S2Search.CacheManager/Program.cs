using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models;
using S2Search.Backend.Services;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Providers;
using S2Search.CacheManager.Interfaces.Managers;
using S2Search.CacheManager.Interfaces.Processors;
using S2Search.CacheManager.Managers;
using S2Search.CacheManager.Processors;
using S2Search.CacheManager.Services;
using StackExchange.Redis;

namespace CacheManagerApp
{
    class Program
    {
        private static readonly string CustomerEndpoint = "ui.s2search.local";

        static async Task<int> Main(string[] args)
        {
            using (var host = CreateHostBuilder(args).Build())
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Cache Manager Starting up...");

                // graceful shutdown via Ctrl+C
                using (var stoppingToken = new CancellationTokenSource())
                {
                    Console.CancelKeyPress += (s, e) =>
                    {
                        e.Cancel = true;
                        logger.LogInformation("Cancel requested via Console. Shutting down...");
                        stoppingToken.Cancel();
                    };

                    try
                    {
                        // this will run the background service
                        // Let the host run and manage the BackgroundService which calls processor.RunAsync(...)
                        //await host.RunAsync(stoppingToken.Token);

                        // how can i create an instance of IBuildCacheProcessor and run its method ProcessAsync here?
                        // run the build cache processor directly for testing

                        // registers AzureFacetService and all other services from the services project
                        // Start the host to ensure hosted services and other start-up work run if required.
                        await host.StartAsync(stoppingToken.Token).ConfigureAwait(false);

                        // Resolve and run the processor directly for testing.
                        var buildCacheProcessor = host.Services.GetRequiredService<IBuildCacheProcessor>();
                        await buildCacheProcessor.ProcessAsync(CustomerEndpoint).ConfigureAwait(false);

                        // Graceful shutdown
                        await host.StopAsync(CancellationToken.None).ConfigureAwait(false);
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

                    services.AddBackendServices(context.Configuration);

                    // Host should manage lifecycle of the processor via a BackgroundService wrapper.
                    services.AddHostedService<PurgeCache>();

                    // Keep the processor registered so it can be injected into the BackgroundService.
                    services.AddSingleton<IPurgeCacheProcessor, PurgeCacheProcessor>();
                    services.AddSingleton<IBuildCacheProcessor, BuildCacheProcessor>();
                    services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
                    services.AddSingleton<ICacheManager, RedisCacheManager>();
                    services.AddSingleton<IQueueManager, QueueManager>();
                });
        }
    }
}