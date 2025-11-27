# -------------------------------------------------------------------------
# Build stage: use the .NET SDK to restore and publish
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copy the entire solution structure (Domain, Services, APIs folders)
COPY . .

# 2. Restore all project dependencies by referencing the API project
RUN dotnet restore APIs/S2Search.API/S2Search.CacheManager.csproj

# 3. Publish the specific API project
RUN dotnet publish APIs/S2Search.API/S2Search.CacheManager.csproj -c Release -o /app/publish /p:UseAppHost=false

# -------------------------------------------------------------------------
# Runtime stage: use the smaller ASP.NET runtime image
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# ** SECURITY IMPROVEMENT 1: Use a non-root user **
# This ensures the container process does not run with elevated privileges.
# The 'app' user is often built into the base image.
USER app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# ** FIX: Use ASPNETCORE_HTTP_PORT for reliable Kestrel binding to port 8080 **
# This environment variable is preferred for container environments.
ENV ASPNETCORE_HTTP_PORT 8080
EXPOSE 8080

# NOTE: Removed EXPOSE 443. If you require HTTPS, it's standard practice
# to handle SSL termination at the Ingress Controller (e.g., Nginx, Application Gateway)

# Use the dll produced by dotnet publish
ENTRYPOINT ["dotnet", "CacheManager.dll"]