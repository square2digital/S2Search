using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using S2.Test.Importer.Services;
using S2.Test.Importer.Helpers;
using S2.Test.Importer.Data.Synonyms;
using S2.Test.Importer;

var serviceProvider = new Startup().ConfigureServices();

await ExecuteStepsAsync(serviceProvider);

static async Task ExecuteStepsAsync(IServiceProvider serviceProvider)
{
    Console.ResetColor();
    var dataImport = serviceProvider.GetRequiredService<IDataImport>();
    var generateSynonyms = serviceProvider.GetRequiredService<IGenerateSynonyms>();
    var appSettings = serviceProvider.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value;
    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Importer");

    logger.LogDebug(appSettings.IndexSettings.SearchIndexName);
    logger.LogDebug(appSettings.APIKeys.QueryKey);

    ConsoleHelper.WriteIndicatorMessage($"Calling Cleanup Existing Resources on index {appSettings.IndexSettings.SearchIndexName}...\n");

    try
    {
        ConsoleHelper.WriteIndicatorMessage($"Cleaning Resources...\n");
        dataImport.CleanupResources();
        Console.WriteLine($"Resources cleaned successfully...\n");
    }
    catch (Exception ex)
    {
        ConsoleHelper.WriteErrorMessage(ex.ToString());
    }

    try
    {
        ConsoleHelper.WriteIndicatorMessage($"Uploading Synonyms...\n");
        generateSynonyms.UploadSynonyms();
        Console.WriteLine($"Uploaded Synonyms successfully...\n");
    }
    catch (Exception ex)
    {
        ConsoleHelper.WriteErrorMessage(ex.ToString());
    }

    try
    {
        ConsoleHelper.WriteIndicatorMessage($"Creating {appSettings.IndexSettings.SearchIndexName} index...\n");
        dataImport.CreateVehiclesIndex();
        ConsoleHelper.WriteIndicatorMessage($"{appSettings.IndexSettings.SearchIndexName} created successfully...\n");
    }
    catch (Exception ex)
    {
        ConsoleHelper.WriteErrorMessage(ex.ToString());
    }

    try
    {
        ConsoleHelper.WriteIndicatorMessage($"Uploading Vehicle Documents...\n");
        dataImport.UploadVehicleDocuments();
        ConsoleHelper.WriteIndicatorMessage($"Vehicle Documents uploaded successfully...\n");
    }
    catch (Exception ex)
    {
        ConsoleHelper.WriteErrorMessage(ex.ToString());
    }

    try
    {
        ConsoleHelper.WriteIndicatorMessage($"Enabling synonyms in the test index...\n");
        generateSynonyms.EnableSynonymsInVehicleIndexSafely();

        ConsoleHelper.WriteInformationMessage($"Waiting for the changes to propagate - stand by...\n");
        await Task.Delay(10000);
        ConsoleHelper.WriteInformationMessage($"Changes propagated successfully\n");
    }
    catch (Exception ex)
    {
        ConsoleHelper.WriteErrorMessage(ex.ToString());
    }

    Console.ResetColor();
    ConsoleHelper.WriteIndicatorMessage("Process Complete\n");
}