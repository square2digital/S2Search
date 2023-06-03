using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S2.Test.Importer.Services;
using Microsoft.Extensions.Options;
using System.Threading;
using S2.Test.Importer.Helpers;
using S2.Test.Importer.Data.Synonyms;

namespace S2.Test.Importer
{
    public class Program
    {
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Startup startup = new Startup();         

            IServiceCollection ServiceCollection = startup.ConfigureServices();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            ExecuteSteps(serviceProvider);
        }

        /// <summary>
        /// This EXE will do the following steps...
        /// 1 - Delete all existing test data
        /// 2 - Create the Demo-Vehicles Index
        /// 3 - Upload the vehicle data from the JSON file "Demo Vehicles.json"
        /// 4 - Create some Synonyms (Investigate further) - we also need new cars - some VWs would be good (bimma and zimma)
        /// 5 - Run test queries (TODO)
        /// Complete
        /// </summary>
        private static void ExecuteSteps(ServiceProvider serviceProvider)
        {
            Console.ResetColor();
            var DataImport = serviceProvider.GetService<IDataImport>();
            var GenerateSynonyms = serviceProvider.GetService<IGenerateSynonyms>();
            var AppSettings = serviceProvider.GetService<IOptionsSnapshot<AppSettings>>().Value;
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogDebug(AppSettings.IndexSettings.SearchIndexName);
            logger.LogDebug(AppSettings.APIKeys.QueryKey);

            ConsoleHelper.WriteIndicatorMessage($"Calling Cleanup Existing Resources on index {AppSettings.IndexSettings.SearchIndexName}...\n");

            try
            {
                ConsoleHelper.WriteIndicatorMessage($"Cleaning Resources...\n");
                DataImport.CleanupResources();
                Console.WriteLine($"Resources cleaned successfully...\n");
            }
            catch(Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
                throw;
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage($"Uploading Synonyms...\n");
                GenerateSynonyms.UploadSynonyms();
                Console.WriteLine($"Uploaded Synonyms successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
                throw;
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage($"Creating {AppSettings.IndexSettings.SearchIndexName} index...\n");
                DataImport.CreateVehiclesIndex();
                ConsoleHelper.WriteIndicatorMessage($"{AppSettings.IndexSettings.SearchIndexName} created successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
                throw;
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage($"Uploading Vehicle Documents...\n");
                DataImport.UploadVehicleDocuments();
                ConsoleHelper.WriteIndicatorMessage($"Vehicle Documents uploaded successfully...\n");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
                throw;
            }

            try
            {
                ConsoleHelper.WriteIndicatorMessage($"Enabling synonyms in the test index...\n");
                GenerateSynonyms.EnableSynonymsInVehicleIndexSafely();

                ConsoleHelper.WriteInformationMessage($"Waiting for the changes to propagate - stand by...\n");
                Thread.Sleep(10000);
                ConsoleHelper.WriteInformationMessage($"Changes propagated sucessfully\n");
            }

            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorMessage(ex.ToString());
                throw;
            }

            Console.ResetColor();
            ConsoleHelper.WriteIndicatorMessage("Process Complete\n");
            Environment.Exit(0);
        }
    }
}