# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated9.0 AS base
USER app
WORKDIR /home/site/wwwroot
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies (better Docker layer caching)
# Copy the main function project
COPY ["AzureFunctions/S2Search.Function.SearchInsights/S2Search.Function.SearchInsights.csproj", "./AzureFunctions/S2Search.Function.SearchInsights/"]

# Copy the dependency projects (correct paths based on workspace structure)
COPY ["Domain/S2Search.Backend.Domain/S2Search.Backend.Domain.csproj", "./Domain/S2Search.Backend.Domain/"]
COPY ["Services/S2Search.Backend.Services/S2Search.Backend.Services.csproj", "./Services/S2Search.Backend.Services/"]

# Restore dependencies for the function project (this will restore all dependencies including referenced projects)
RUN dotnet restore "./AzureFunctions/S2Search.Function.SearchInsights/S2Search.Function.SearchInsights.csproj" --disable-parallel

# Copy all source code
COPY AzureFunctions/S2Search.Function.SearchInsights/ ./AzureFunctions/S2Search.Function.SearchInsights/
COPY Domain/ ./Domain/
COPY Services/ ./Services/

# Set working directory to the function project and build it (this will build all dependencies)
WORKDIR "/src/AzureFunctions/S2Search.Function.SearchInsights"
RUN dotnet build "S2Search.Function.SearchInsights.csproj" -c $BUILD_CONFIGURATION --no-restore

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/AzureFunctions/S2Search.Function.SearchInsights"
RUN dotnet publish "S2Search.Function.SearchInsights.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false \
    /p:PublishReadyToRun=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /home/site/wwwroot

# Copy published application
COPY --from=publish /app/publish .

# Set Azure Functions environment variables
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true \
    FUNCTIONS_WORKER_RUNTIME=dotnet-isolated \
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    ASPNETCORE_URLS=http://+:8080

# Health check for container orchestration
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/api/health || exit 1