using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using S2Search.ClientConfiguration.API.Filters;
using S2Search.Common.Database.Sql.Dapper.Utilities;
using Services;
using System;
using System.IO;
using System.Reflection;

namespace S2Search.ClientConfiguration.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public readonly string CorsPolicyName = "CorsPolicy";
        public IWebHostEnvironment _env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(options =>
            {
                options.DeveloperMode = _env.IsDevelopment();
            });
            services.AddApplicationInsightsTelemetryProcessor<ProbeEndpointFilter>();

            services.AddLogging(options =>
            {
                options.AddConsole();
                options.AddDebug();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                    builder.WithOrigins("http://localhost:3001");
                    builder.WithOrigins("http://localhost:8080");
                    builder.WithOrigins("http://www.s2search.local");
                    builder.WithOrigins("https://s2searchui.azurewebsites.net");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client Configuration API", Version = "v1.0.0" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddAPIServices();
            services.AddSqlDapperAbstractions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
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
        }
    }
}
