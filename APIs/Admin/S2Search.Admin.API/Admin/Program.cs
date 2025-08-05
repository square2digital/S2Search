using Domain.AppSettings;
using Microsoft.OpenApi.Models;
using Services.Configuration.Interfaces.Repositories;
using Services.Configuration.Repositories;
using Services.Customer.Interfaces.Managers;
using Services.Customer.Managers;
using Services.Customer.Interfaces.Providers;
using Services.Customer.Providers;
using Services.Customer.Interfaces.Repositories;
using Services.Customer.Repositories;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Dapper.Providers;

namespace S2Search.Admin.API
{
    public class Program
    {
        public static IConfiguration? Configuration { get; }
        public static IWebHostEnvironment? Env { get; }

        public static readonly string CorsPolicyName = "CorsPolicy";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(CorsPolicyName);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "S2 Admin API v1");
            });

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

            // Uncomment if ProbeEndpointFilter is implemented
            // services.AddApplicationInsightsTelemetryProcessor<ProbeEndpointFilter>();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.WithOrigins("http://localhost:3001", "http://localhost:3000")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddResponseCaching();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "S2 Admin API", Version = "v1.0.0" });
                // Uncomment if AdditionalPropertiesDocumentFilter is implemented
                // c.DocumentFilter<AdditionalPropertiesDocumentFilter>();
            });

            services.AddHttpContextAccessor();
            services.AddLazyCache();

            // AddServices
            services.AddSingleton<ISearchIndexRepository, SearchIndexRepository>();
            services.AddSingleton<IThemeRepository, ThemeRepository>();
            services.AddSingleton<ISearchConfigurationRepository, SearchConfigurationRepository>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDbContextProvider, DbContextProvider>();

            // AddManagers
            services.AddSingleton<IQueueManager, QueueManager>();
            services.AddSingleton<IFeedSettingsValidationManager, FeedSettingsValidationManager>();
            services.AddSingleton<ICronDescriptionManager, CronDescriptionManager>();
            services.AddSingleton<IQueryKeyNameValidationManager, QueryKeyNameValidationManager>();
            services.AddSingleton<INotificationRuleValidationManager, NotificationRuleValidationManager>();
            services.AddSingleton<ISearchInterfaceValidationManager, SearchInterfaceValidationManager>();
            services.AddSingleton<ISynonymValidationManager, SynonymValidationManager>();
            services.AddSingleton<ISolrFormatConversionManager, SolrFormatConversionManager>();
            services.AddSingleton<IFeedCredentialsManager, FeedCredentialsManager>();
            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddSingleton<IDashboardManager, DashboardManager>();
            services.AddSingleton<IFeedUploadDestinationManager, FeedUploadDestinationManager>();
            services.AddSingleton<IFeedUploadValidationManager, FeedUploadValidationManager>();
            services.AddSingleton<IFeedUploadManager, FeedUploadManager>();
            services.AddSingleton<ISearchInsightsRetrievalManager, SearchInsightsRetrievalManager>();

            // Providers
            services.AddSingleton<IGuidProvider, GuidProvider>();
            services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IBlobClientProvider, BlobClientProvider>();
            services.AddSingleton<IPreviousDateRangeProvider, PreviousDateRangeProvider>();
            services.AddSingleton<IPercentageChangeProvider, PercentageChangeProvider>();
            services.AddSingleton<ISearchInsightFriendlyNameProvider, SearchInsightFriendlyNameProvider>();

            // AddRepositories
            services.AddSingleton<IFeedRepository, FeedRepository>();
            services.AddSingleton<INotificationRuleRepository, NotificationRuleRepository>();
            services.AddSingleton<ISearchInterfaceRepository, SearchInterfaceRepository>();
            services.AddSingleton<ISynonymRepository, SynonymRepository>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<IFeedCredentialsRepository, FeedCredentialsRepository>();
            services.AddSingleton<INotificationRepository, NotificationRepository>();
            services.AddSingleton<IDashboardRepository, DashboardRepository>();
            services.AddSingleton<ISearchInsightsRepository, SearchInsightsRepository>();
            services.AddSingleton<ISearchInsightsReportRepository, SearchInsightsReportRepository>();
        }
    }
}