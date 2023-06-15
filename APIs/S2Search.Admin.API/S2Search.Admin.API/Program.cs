using Services;
using System.Reflection;
using Domain.AppSettings;
using S2Search.ClientConfiguration.API.Filters;
using Microsoft.OpenApi.Models;

namespace S2Search.Admin.API
{
    public class Program
    {
        public readonly string CorsPolicyName = "CorsPolicy";

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

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<SftpSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection("SftpSettings").Bind(settings);
                    });

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.DeveloperMode = Env.IsDevelopment();
            });
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Resource API", Version = "v1.0.0" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DocumentFilter<AdditionalPropertiesDocumentFilter>();
            });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddLazyCache();
            services.AddAPIServices();
            services.AddSqlDapperAbstractions();
        }
    }
}