using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S2.Test.Importer.Services;
using S2.Test.Importer.Data.Synonyms;
using S2.Importer.Providers.AzureSearch;
using LazyCache;
using Microsoft.Extensions.Configuration.Json; // Fix for AddJsonFile
using Microsoft.Extensions.Options; // Fix for options binding
using System.IO; // ADD THIS USING DIRECTIVE FOR Directory
using Microsoft.Extensions.Configuration.FileExtensions; // ADD THIS USING DIRECTIVE FOR SetBasePath

namespace S2.Test.Importer
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(basePath, "appsettings.json"), optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IServiceCollection ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Add logging
            serviceCollection.AddLogging();

            // Add configuration binding (use Configure<T>)
            serviceCollection.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add DI types here
            serviceCollection.AddSingleton<IAzureSearchDocumentsClientProvider, AzureSearchDocumentsClientProvider>();
            serviceCollection.AddSingleton<IDataImport, DataImport>();
            serviceCollection.AddSingleton<IGenerateSynonyms, GenerateSynonyms>();

            // Add LazyCache if package is installed
            serviceCollection.AddSingleton<IAppCache, CachingService>();

            return serviceCollection;
        }
    }
}