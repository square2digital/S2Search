using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Providers;
using Services.Interfaces.Managers;
using Services.Interfaces.Mappers;
using Services.Interfaces.Providers;
using Services.Interfaces.Repositories;
using Services.Managers;
using Services.Mappers.FeedTypes;
using Services.Providers;
using Services.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Application Insights for logging and monitoring
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddLazyCache();

        // Register BlobServiceClient with DI, pulling connection string from settings or environment variables
        services.AddSingleton(services =>
        {
            var blobConnectionString = Environment.GetEnvironmentVariable("AzureStorageAccount");
            return new BlobServiceClient(blobConnectionString);
        });

        services
            .AddSingleton<IFeedTargetQueueManager, FeedTargetQueueManager>()
            .AddSingleton<IZipArchiveValidationManager, ZipArchiveValidationManager>()
            .AddSingleton<IFeedProcessingManager, FeedProcessingManager>()
            .AddSingleton<IAzureSearchDocumentsClientProvider, AzureSearchDocumentsClientProvider>()
            .AddSingleton<IAzureSearchManager, AzureSearchManager>()
            .AddSingleton<ICsvParserManager, TinyCsvParserManager>()
            .AddSingleton<IFeedMapperProvider, FeedMapperProvider>()
            .AddSingleton<IFeedRepository, FeedRepository>()
            .AddSingleton<ISearchIndexCredentialsRepository, SearchIndexCredentialsRepository>()
            .AddSingleton<IGenericSynonymRepository, GenericSynonymRepository>()
            .AddSingleton<IFeedMapper, DefaultFeedMapper>()
            .AddSingleton<IFeedMapper, DMS14FeedMapper>()
            .AddSingleton<IConnectionStringProvider, ConnectionStringProvider>()
            .AddSingleton<IDbConnectionFactory, DbConnectionFactory>()
            .AddSingleton<IDbContextProvider, DbContextProvider>();
    })
    .Build();

host.Run();
