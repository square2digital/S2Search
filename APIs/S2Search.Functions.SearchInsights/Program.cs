using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using S2Search.Backend.Services.Functions.SearchInsights.Extensions;

var builder = FunctionsApplication.CreateBuilder(args);

// Register function-specific services (managers, repositories, providers)
builder.Services.AddFunctionServices();

// Ensure logging is available for DI
builder.Services.AddLogging();

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
