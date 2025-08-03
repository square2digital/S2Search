using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Services;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Interfaces;
using Search.Filters;
<<<<<<< HEAD:SearchAPIs/AzureCognitiveServices/S2Search.Search.API/Search/Startup.cs
using S2SearchAPI.Client;
using System.Net.Http;
=======
>>>>>>> master:APIs/Search/AzureCognitiveServices/S2Search.Search.API/Search/Startup.cs

namespace Search
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public readonly string CorsPolicyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<ProbeEndpointFilter>();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
            });

            services.AddResponseCaching();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile
                {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Search API", Version = "v1" });
            });

            services.AddMvcCore().AddApiExplorer();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // caching response for middlewares
            services.AddResponseCaching();

            var appSettings = LoadAppSettings(services);

            services.AddHttpContextAccessor();
            services.AddAPIServices();
            services.AddRedis(appSettings.RedisCacheSettings.RedisConnectionString);

<<<<<<< HEAD:SearchAPIs/AzureCognitiveServices/S2Search.Search.API/Search/Startup.cs
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();

            services.AddSingleton<IS2SearchAPIClient>(provider =>
            {
                var baseUrl = appSettings.AdminSettings.AdminEndpoint;
                var httpClient = provider.GetRequiredService<HttpClient>();

                var s2SearchApiClient = new S2SearchAPIClient(baseUrl, httpClient);

                return s2SearchApiClient;
            });

            // Register the NSwag client
            //services.AddHttpClient<IS2SearchAPIClient, S2SearchAPIClient>()
            //    .ConfigureHttpClient((serviceProvider, httpClient) =>
            //    {
            //        httpClient.BaseAddress = new Uri(appSettings.AdminSettings.AdminEndpoint);
            //    });
=======
            // Add HttpClient to the DI container with a named client (optional but useful for multiple clients)
            services.AddHttpClient("AdminClient");
>>>>>>> master:APIs/Search/AzureCognitiveServices/S2Search.Search.API/Search/Startup.cs
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicyName);

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search Service v1");
            });

            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };

                await next();
            });
        }

        private IAppSettings LoadAppSettings(IServiceCollection services)
        {
            IAppSettings appSettings = new AppSettings();
            appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddSingleton(appSettings);

            return appSettings;
        }
    }
}