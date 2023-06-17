using System.Reflection;
using Domain.AppSettings;
using S2Search.ClientConfiguration.API.Filters;
using Microsoft.OpenApi.Models;
using CustomerResource.Filters;
using Services.Configuration.Interfaces.Repositories;
using Services.Configuration.Repositories;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Providers;

namespace S2Search.Admin.API
{
    public class Program
    {
        public static IConfiguration Configuration { get; }
        public static IWebHostEnvironment Env { get; }

        public static readonly string CorsPolicyName = "CorsPolicy";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // ***************************
            // from the previous services
            // ***************************
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicyName);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client Configuration v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<SftpSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("SftpSettings").Bind(settings);
            });

            //services.AddApplicationInsightsTelemetry(options =>
            //{
            //    options.DeveloperMode = env.IsDevelopment();
            //});
            services.AddApplicationInsightsTelemetryProcessor<ProbeEndpointFilter>();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3001");
                    builder.WithOrigins("http://localhost:3000");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            services.AddResponseCaching();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "S2 Search API", Version = "v1.0.0" });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                c.DocumentFilter<AdditionalPropertiesDocumentFilter>();
            });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddLazyCache();
            //services.AddAPIServices();
            //services.AddSqlDapperAbstractions();

            AddServices(services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<ISearchIndexRepository, SearchIndexRepository>();
            services.AddSingleton<IThemeRepository, ThemeRepository>();
            services.AddSingleton<ISearchConfigurationRepository, SearchConfigurationRepository>();

            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDbContextProvider, DbContextProvider>();
        }
    }
}