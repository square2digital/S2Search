using Domain.Interfaces;
using Domain.Models;
using Services.Helper;
using Services.Helpers.FacetOverrides;
using Services.Helpers;
using Services.Interfaces.FacetOverrides;
using Services.Interfaces;
using Services.Providers;
using Services.Services;

namespace Search
{
    public class Program
    {
        private static IAppSettings _appSettings;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigureAppSettings(builder.Services);
            AddDependancies(builder.Services);
            AddServices(builder.Services);
            AddProviders(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureAppSettings(IServiceCollection services)
        {
            var appsettingsFile = $"appsettings.json";
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Environment configured as {env}");

            if(env == "Development")
            {
                appsettingsFile = $"appsettings.{env}.json";
            }

            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            IConfiguration config = configBuilder.AddJsonFile(appsettingsFile).Build();

            _appSettings = config.GetRequiredSection("AppSettings").Get<AppSettings>();

            services.AddSingleton(_appSettings);
        }

        private static IServiceCollection AddDependancies(IServiceCollection services)
        {
            services.AddSingleton<IElasticSearchClientProvider, ElasticSearchClientProvider>();
            services.AddSingleton<IElasticSearchService, ElasticSearchService>();

            return services;
        }

        private static IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IElasticFacetService, ElasticFacetService>();
            services.AddTransient<ILuceneSyntaxHelper, LuceneSyntaxHelper>();
            services.AddSingleton<IElasticSearchService, ElasticSearchService>();
            services.AddSingleton<IFacetHelper, FacetHelper>();
            services.AddSingleton<IDisplayTextFormatHelper, DisplayTextFormatHelper>();
            services.AddSingleton<IFacetOverride, EngineSizeOverride>();
            services.AddSingleton<IFacetOverride, MileageOverride>();
            services.AddSingleton<IFacetOverride, DoorsOverride>();
            services.AddSingleton<IFacetOverrideProvider, FacetOverrideProvider>();
            services.AddSingleton<ISynonymsService, SynonymsService>();
            services.AddSingleton<IElasticIndexService, ElasticIndexService>();

            return services;
        }

        private static IServiceCollection AddProviders(IServiceCollection services)
        {
            services.AddSingleton<ISynonymsHelper, SynonymsHelper>();

            return services;
        }
    }
}