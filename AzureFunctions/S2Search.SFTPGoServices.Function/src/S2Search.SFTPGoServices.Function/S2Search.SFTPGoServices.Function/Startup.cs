using Domain.AppSettings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using S2Search.SFTPGoServices.Function;
using Services;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]

namespace S2Search.SFTPGoServices.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureOptions(builder);

            builder.Services.AddFunctionServices();
        }

        private static void ConfigureOptions(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<AzureSettings>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection("AzureSettings").Bind(settings);
                            });
            builder.Services.AddOptions<SFTPGoSettings>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection("SFTPGoSettings").Bind(settings);
                            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }
    }
}
