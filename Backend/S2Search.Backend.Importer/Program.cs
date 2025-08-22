using LazyCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S2.Importer.Providers.AzureSearch;
using S2.Test.Importer;
using S2.Test.Importer.Data.Synonyms;
using S2.Test.Importer.Helpers;
using S2.Test.Importer.Services;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // Remove this line if AddConsole is not available or not working
        services.AddLogging(); // Registers ILoggerFactory and basic logging services

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        // Register configuration binding
        //services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        // Add DI types
        services.AddSingleton<IAzureSearchDocumentsClientProvider, AzureSearchDocumentsClientProvider>();
        services.AddSingleton<IDataImport, DataImport>();
        services.AddSingleton<IGenerateSynonyms, GenerateSynonyms>();

        // Add LazyCache
        services.AddSingleton<IAppCache, CachingService>();

        var serviceProvider = services.BuildServiceProvider();

        await ExecuteStepsAsync(serviceProvider);

        static async Task ExecuteStepsAsync(IServiceProvider serviceProvider)
        {
            Console.ResetColor();

            var dataImport = serviceProvider.GetRequiredService<IDataImport>();
            var generateSynonyms = serviceProvider.GetRequiredService<IGenerateSynonyms>();
            // var appSettings = serviceProvider.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value;
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Importer");

            // logger.LogDebug(appSettings.IndexSettings.SearchIndexName);
            // logger.LogDebug(appSettings.APIKeys.QueryKey);

            try
            {
                ConsoleHelper.WriteIndicatorMessage("Cleaning Resources...\n");
                dataImport.CleanupResources();
                Console.WriteLine("Resources cleaned successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage("Uploading Synonyms...\n");
                generateSynonyms.UploadSynonyms();
                Console.WriteLine("Uploaded Synonyms successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage("Creating index...\n");
                dataImport.CreateVehiclesIndex();
                ConsoleHelper.WriteIndicatorMessage("Index created successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage("Uploading Vehicle Documents...\n");
                dataImport.UploadVehicleDocuments();
                ConsoleHelper.WriteIndicatorMessage("Vehicle Documents uploaded successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage("Enabling synonyms in the test index...\n");
                generateSynonyms.EnableSynonymsInVehicleIndexSafely();

                ConsoleHelper.WriteInformationMessage("Waiting for the changes to propagate - stand by...\n");
                await Task.Delay(10000);
                ConsoleHelper.WriteInformationMessage("Changes propagated successfully\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
            }

            Console.ResetColor();
            ConsoleHelper.WriteIndicatorMessage("Process Complete\n");
        }
    }
}