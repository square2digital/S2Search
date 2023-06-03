using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S2.Test.Importer.Services;
using S2.Test.Importer.Data.Synonyms;
using S2.Importer.Providers.AzureSearch;

namespace S2.Test.Importer
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            ConfigureServices();
        }

        public IServiceCollection ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
            });

            serviceCollection.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add DI types here
            serviceCollection.AddSingleton<IAzureSearchDocumentsClientProvider, AzureSearchDocumentsClientProvider>();
            serviceCollection.AddSingleton<IDataImport, DataImport>();
            serviceCollection.AddSingleton<IGenerateSynonyms, GenerateSynonyms>();

            serviceCollection.AddLazyCache();

            return serviceCollection;
        }
    }
}